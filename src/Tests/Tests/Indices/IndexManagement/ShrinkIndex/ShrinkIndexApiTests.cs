﻿using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using FluentAssertions;
using Nest6;
using Tests.Core.Extensions;
using Tests.Core.ManagedElasticsearch.Clusters;
using Tests.Framework;
using Tests.Framework.Integration;

namespace Tests.Indices.IndexManagement.ShrinkIndex
{
	public class ShrinkIndexApiTests
		: ApiIntegrationTestBase<WritableCluster, IShrinkIndexResponse, IShrinkIndexRequest, ShrinkIndexDescriptor, ShrinkIndexRequest>
	{
		public ShrinkIndexApiTests(WritableCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override bool ExpectIsValid => true;

		protected override object ExpectJson { get; } = new
		{
			settings = new Dictionary<string, object>
			{
				{ "index.number_of_shards", 4 }
			}
		};

		protected override int ExpectStatusCode => 200;

		protected override Func<ShrinkIndexDescriptor, IShrinkIndexRequest> Fluent => d => d
			.Settings(s => s
				.NumberOfShards(4)
			);

		protected override HttpMethod HttpMethod => HttpMethod.PUT;

		protected override ShrinkIndexRequest Initializer => new ShrinkIndexRequest(CallIsolatedValue, CallIsolatedValue + "-target")
		{
			Settings = new Nest6.IndexSettings
			{
				NumberOfShards = 4
			}
		};

		protected override string UrlPath => $"/{CallIsolatedValue}/_shrink/{CallIsolatedValue}-target";

		protected override LazyResponses ClientUsage() => Calls(
			(client, f) => client.ShrinkIndex(CallIsolatedValue, CallIsolatedValue + "-target", f),
			(client, f) => client.ShrinkIndexAsync(CallIsolatedValue, CallIsolatedValue + "-target", f),
			(client, r) => client.ShrinkIndex(r),
			(client, r) => client.ShrinkIndexAsync(r)
		);

		protected override void OnBeforeCall(IElasticClient client)
		{
			var create = client.CreateIndex(CallIsolatedValue, c => c
				.Settings(s => s
					.NumberOfShards(8)
					.NumberOfReplicas(0)
				)
			);
			create.ShouldBeValid();
			var update = client.UpdateIndexSettings(CallIsolatedValue, u => u
				.IndexSettings(s => s
					.BlocksWrite()
				)
			);
			update.ShouldBeValid();
		}

		protected override ShrinkIndexDescriptor NewDescriptor() => new ShrinkIndexDescriptor(CallIsolatedValue, CallIsolatedValue + "-target");

		protected override void ExpectResponse(IShrinkIndexResponse response)
		{
			response.ShouldBeValid();
			response.Acknowledged.Should().BeTrue();
			response.ShardsAcknowledged.Should().BeTrue();
		}
	}
}
