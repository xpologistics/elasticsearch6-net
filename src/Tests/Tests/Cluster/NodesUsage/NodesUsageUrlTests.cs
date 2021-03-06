﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Elasticsearch.Net;
using Nest6;
using Tests.Framework;
using static Tests.Framework.UrlTester;

namespace Tests.XPack.NodesUsage
{
	public class NodesUsageUrlTests : UrlTestsBase
	{
		[U] public override async Task Urls()
		{
			await GET("/_nodes/usage")
					.Fluent(c => c.NodesUsage(d => d))
					.Request(c => c.NodesUsage(new NodesUsageRequest()))
					.FluentAsync(c => c.NodesUsageAsync(d => d))
					.RequestAsync(c => c.NodesUsageAsync(new NodesUsageRequest()))
				;

			await GET("/_nodes/nodeId/usage")
					.Fluent(c => c.NodesUsage(d => d.NodeId("nodeId")))
					.Request(c => c.NodesUsage(new NodesUsageRequest("nodeId")))
					.FluentAsync(c => c.NodesUsageAsync(d => d.NodeId("nodeId")))
					.RequestAsync(c => c.NodesUsageAsync(new NodesUsageRequest("nodeId")))
				;

			await GET("/_nodes/nodeId/usage/rest_actions")
					.Fluent(c => c.NodesUsage(d => d.NodeId("nodeId").Metric(NodesUsageMetric.RestActions)))
					.Request(c => c.NodesUsage(new NodesUsageRequest("nodeId", NodesUsageMetric.RestActions)))
					.FluentAsync(c => c.NodesUsageAsync(d => d.NodeId("nodeId").Metric(NodesUsageMetric.RestActions)))
					.RequestAsync(c => c.NodesUsageAsync(new NodesUsageRequest("nodeId", NodesUsageMetric.RestActions)))
				;

			await GET("/_nodes/nodeId/usage/_all")
					.Fluent(c => c.NodesUsage(d => d.NodeId("nodeId").Metric(NodesUsageMetric.All)))
					.Request(c => c.NodesUsage(new NodesUsageRequest("nodeId", NodesUsageMetric.All)))
					.FluentAsync(c => c.NodesUsageAsync(d => d.NodeId("nodeId").Metric(NodesUsageMetric.All)))
					.RequestAsync(c => c.NodesUsageAsync(new NodesUsageRequest("nodeId", NodesUsageMetric.All)))
				;
		}
	}
}
