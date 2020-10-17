using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elastic.Managed.Ephemeral;
using Elastic.Xunit.XunitPlumbing;
using Elasticsearch.Net;
using FluentAssertions;
using Nest6;
using Tests.Core.ManagedElasticsearch.Clusters;
using Tests.Framework;
using Tests.Framework.EndpointTests.TestState;
using Tests.Framework.Integration;

namespace Tests.XPack.Security.Privileges
{
	[SkipVersion("<6.5.0", "All APIs exist in Elasticsearch 6.4.0, except Has Privileges, which is a 6.50+ feature")]
	public class ApplicationPrivilegesApiTests : CoordinatedIntegrationTestBase<XPackCluster>
	{
		private const string PutPrivilegesStep = nameof(PutPrivilegesStep);
		private const string GetPrivilegesStep = nameof(GetPrivilegesStep);
		private const string GetUserPrivilegesStep = nameof(GetUserPrivilegesStep);
		private const string PutRoleStep = nameof(PutRoleStep);
		private const string PutUserStep = nameof(PutUserStep);
		private const string HasPrivilegesStep = nameof(HasPrivilegesStep);
		private const string DeletePrivilegesStep = nameof(DeletePrivilegesStep);

		public ApplicationPrivilegesApiTests(XPackCluster cluster, EndpointUsage usage) : base(new CoordinatedUsage(cluster, usage)
		{
			{
				PutPrivilegesStep, u => u.Calls<PutPrivilegesDescriptor, PutPrivilegesRequest, IPutPrivilegesRequest, IPutPrivilegesResponse>(
					v => new PutPrivilegesRequest
					{
						Applications = new AppPrivileges
						{
							{
								$"app-{v}", new Nest.Privileges
								{
									{
										$"p1-{v}", new PrivilegesActions
										{
											Actions = new[] { "data:read/*", "action:login" },
											Metadata = new Dictionary<string, object>
											{
												{ "key1", "val1a" },
												{ "key2", "val2a" }
											}
										}
									}
								}
							}
						}
					},
					(v, d) => d
						.Applications(a => a
							.Application($"app-{v}", pr => pr
								.Privilege($"p1-{v}", ac => ac
									.Actions("data:read/*", "action:login")
									.Metadata(m => m
										.Add("key1", "val1a")
										.Add("key2", "val2a")
									)
								)
							)
						),
					(v, c, f) => c.PutPrivileges(f),
					(v, c, f) => c.PutPrivilegesAsync(f),
					(v, c, r) => c.PutPrivileges(r),
					(v, c, r) => c.PutPrivilegesAsync(r)
				)
			},
			{
				GetPrivilegesStep, u => u.Calls<GetPrivilegesDescriptor, GetPrivilegesRequest, IGetPrivilegesRequest, IGetPrivilegesResponse>(
					v => new GetPrivilegesRequest($"app-{v}", $"p1-{v}"),
					(v, d) => d.Application($"app-{v}").Name($"p1-{v}"),
					(v, c, f) => c.GetPrivileges(f),
					(v, c, f) => c.GetPrivilegesAsync(f),
					(v, c, r) => c.GetPrivileges(r),
					(v, c, r) => c.GetPrivilegesAsync(r)
				)
			},
			{
				PutRoleStep, u => u.Call((v, c) => c.PutRoleAsync($"role-{v}", r=>r
					.Applications(apps=>apps
						.Add<object>(a=>a.Resources("*").Privileges($"p1-{v}").Application($"app-{v}"))
					)
				))
			},
			{
				PutUserStep, u => u.Call((v, c) => c.PutUserAsync($"user-ap-{v}",
					r => r.Roles("admin", $"role-{v}").Password($"pass-{v}")))
			},
			{
				HasPrivilegesStep, u => u.Calls<HasPrivilegesDescriptor, HasPrivilegesRequest, IHasPrivilegesRequest, IHasPrivilegesResponse>(
					v => new HasPrivilegesRequest
					{
						RequestConfiguration = new RequestConfiguration
						{
							BasicAuthenticationCredentials = new BasicAuthenticationCredentials
							{
								Username = $"user-ap-{v}", Password = $"pass-{v}"
							}
						},
						Application = new[]
						{
							new ApplicationPrivilegesCheck
							{
								Name = $"app-{v}",
								Privileges = new[] { "action:login" },
								Resources = new [] {"*"}
							}
						}
					},
					(v, d) => d
						.RequestConfiguration(r=>r.BasicAuthentication($"user-ap-{v}", $"pass-{v}"))
						.Applications(apps => apps
							.Application(a => a
								.Name($"app-{v}")
								.Privileges("action:login")
								.Resources($"p1-{v}")
							)
						)
					,
					(v, c, f) => c.HasPrivileges(f),
					(v, c, f) => c.HasPrivilegesAsync(f),
					(v, c, r) => c.HasPrivileges(r),
					(v, c, r) => c.HasPrivilegesAsync(r)
				)
			},
			{
				GetUserPrivilegesStep, u => u.Calls<GetUserPrivilegesDescriptor, GetUserPrivilegesRequest, IGetUserPrivilegesRequest, IGetUserPrivilegesResponse>(
					v => new GetUserPrivilegesRequest
					{
						RequestConfiguration = new RequestConfiguration
						{
							BasicAuthenticationCredentials = new BasicAuthenticationCredentials
							{
								Username = $"user-ap-{v}", Password = $"pass-{v}"
						}
						}
					},
					(v, d) => d.RequestConfiguration(r=>r.BasicAuthentication($"user-ap-{v}", $"pass-{v}")),
					(v, c, f) => c.GetUserPrivileges(f),
					(v, c, f) => c.GetUserPrivilegesAsync(f),
					(v, c, r) => c.GetUserPrivileges(r),
					(v, c, r) => c.GetUserPrivilegesAsync(r)
				)
			},
			{
				DeletePrivilegesStep, u => u.Calls<DeletePrivilegesDescriptor, DeletePrivilegesRequest, IDeletePrivilegesRequest, IDeletePrivilegesResponse>(
					v => new DeletePrivilegesRequest($"app-{v}", $"p1-{v}"),
					(v, d) => d,
					(v, c, f) => c.DeletePrivileges($"app-{v}", $"p1-{v}"),
					(v, c, f) => c.DeletePrivilegesAsync($"app-{v}", $"p1-{v}"),
					(v, c, r) => c.DeletePrivileges(r),
					(v, c, r) => c.DeletePrivilegesAsync(r)
				)
			}
		}) { }

		[I] public async Task PutPrivilegesResponse() => await Assert<PutPrivilegesResponse>(PutPrivilegesStep, (v, r) =>
		{
			r.IsValid.Should().BeTrue();
			r.ApiCall.HttpStatusCode.Should().Be(200);
			r.Applications.Should().NotBeEmpty();
			var app = $"app-{v}";
			var hasApp = r.Applications.TryGetValue(app, out var privilegesDict);
			hasApp.Should().BeTrue($"expect `{app}` to be returned");
			privilegesDict.Should().NotBeNull($"expect `{app}`'s value not to be null");

			var privilege = $"p1-{v}";
			var hasP1 = privilegesDict.TryGetValue(privilege, out var createdValue);
			hasP1.Should().BeTrue($"expect `{privilege}` to be returned");
			createdValue.Should().NotBeNull($"expect `{privilege}`'s value not to be null");
			createdValue.Created.Should().BeTrue($"expect `{privilege}` to be created in the response");
		});

		[I] public async Task GetPrivilegesResponse() => await Assert<GetPrivilegesResponse>(GetPrivilegesStep, (v, r) =>
		{
			r.IsValid.Should().BeTrue();
			r.ApiCall.HttpStatusCode.Should().Be(200);
			r.Applications.Should().NotBeEmpty();
			var app = $"app-{v}";
			var hasApp = r.Applications.TryGetValue(app, out var privilegesDict);
			hasApp.Should().BeTrue($"expect `{app}` to be returned");
			privilegesDict.Should().NotBeNull($"expect `{app}`'s value not to be null");

			var privilegeName = $"p1-{v}";
			var hasP1 = privilegesDict.TryGetValue(privilegeName, out var privilege);
			hasP1.Should().BeTrue($"expect `{privilegeName}` to be returned");
			privilege.Should().NotBeNull($"expect `{privilegeName}`'s value not to be null");
			privilege.Actions.Should().NotBeEmpty($"expect `{privilegeName}` to return its actions");
			privilege.Metadata.Should().NotBeEmpty($"expect `{privilegeName}` to return its metadata");
		});

		[I] public async Task PutRoleResponse() => await Assert<PutRoleResponse>(PutRoleStep, (v, r) =>
		{
			r.IsValid.Should().BeTrue();
			r.Role.Should().NotBeNull();
			r.Role.Created.Should().BeTrue();
		});

		[I] public async Task PutUserResponse() => await Assert<PutUserResponse>(PutUserStep, (v, r) =>
		{
			r.IsValid.Should().BeTrue();
			r.User.Should().NotBeNull();
			r.User.Created.Should().BeTrue();
		});

		[I] public async Task HasPrivilegesResponse() => await Assert<HasPrivilegesResponse>(HasPrivilegesStep, (v, r) =>
		{
			r.IsValid.Should().BeTrue();
			r.ApiCall.HttpStatusCode.Should().Be(200);
			r.Username.Should().Be($"user-ap-{v}");
			r.HasAllRequested.Should().Be(true);
			r.Applications.Should().NotBeEmpty();
			var app = $"app-{v}";
			var hasApp = r.Applications.TryGetValue(app, out var privilegesDict);
			hasApp.Should().BeTrue($"expect `{app}` to be returned");
			privilegesDict.Should().NotBeNull($"expect `{app}`'s value not to be null");
		});

		[I] public async Task GetUserPrivilegesResponse() => await Assert<GetUserPrivilegesResponse>(GetUserPrivilegesStep, (v, r) =>
		{
			r.IsValid.Should().BeTrue();

			r.Cluster.Count.Should().Be(1);
			r.Cluster.First().Should().Be("all");

			r.Global.Count.Should().Be(0);

			r.Indices.Count.Should().Be(1);
			var index = r.Indices.First();
			index.Names.Count.Should().Be(1);
			index.Names.First().Should().Be("*");
			index.Privileges.Count.Should().Be(1);
			index.Privileges.First().Should().Be("all");

			r.Applications.Count.Should().Be(1);
			var application = r.Applications.First();
			application.Application.Should().Be($"app-{v}");
			application.Privileges.Count.Should().Be(1);
			application.Privileges.First().Should().Be($"p1-{v}");
			application.Resources.Count.Should().Be(1);
			application.Resources.First().Should().Be("*");

			r.RunAs.Count.Should().Be(0);
		});

		[I] public async Task DeletePrivilegesResponse() => await Assert<DeletePrivilegesResponse>(DeletePrivilegesStep, (v, r) =>
		{
			r.IsValid.Should().BeTrue();

			r.Applications.Should().NotBeEmpty();
			var app = $"app-{v}";
			var hasApp = r.Applications.TryGetValue(app, out var privilegesDict);
			hasApp.Should().BeTrue($"expect `{app}` to be returned");
			privilegesDict.Should().NotBeNull($"expect `{app}`'s value not to be null");

			var privilegeName = $"p1-{v}";
			var hasP1 = privilegesDict.TryGetValue(privilegeName, out var found);
			hasP1.Should().BeTrue($"expect `{privilegeName}` to be returned");
			found.Found.Should().Be(true);
		});
	}
}
