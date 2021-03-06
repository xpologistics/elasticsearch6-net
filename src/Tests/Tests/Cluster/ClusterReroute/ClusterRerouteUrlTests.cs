﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest6;
using Tests.Framework;
using static Tests.Framework.UrlTester;

namespace Tests.Cluster.ClusterReroute
{
	public class ClusterRerouteUrlTests : UrlTestsBase
	{
		[U] public override async Task Urls() => await POST("/_cluster/reroute")
			.Fluent(c => c.ClusterReroute(r => r))
			.Request(c => c.ClusterReroute(new ClusterRerouteRequest()))
			.FluentAsync(c => c.ClusterRerouteAsync(r => r))
			.RequestAsync(c => c.ClusterRerouteAsync(new ClusterRerouteRequest()));
	}
}
