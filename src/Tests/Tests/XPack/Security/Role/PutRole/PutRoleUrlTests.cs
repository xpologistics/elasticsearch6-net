﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest6;
using Tests.Framework;
using static Tests.Framework.UrlTester;

namespace Tests.XPack.Security.Role.PutRole
{
	public class PutRoleUrlTests : UrlTestsBase
	{
		[U] public override async Task Urls() => await PUT("/_xpack/security/role/mpdreamz")
			.Fluent(c => c.PutRole("mpdreamz"))
			.Request(c => c.PutRole(new PutRoleRequest("mpdreamz")))
			.FluentAsync(c => c.PutRoleAsync("mpdreamz"))
			.RequestAsync(c => c.PutRoleAsync(new PutRoleRequest("mpdreamz")));
	}
}
