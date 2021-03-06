﻿using System;
using Elasticsearch.Net;
using FluentAssertions;
using Nest6;
using Tests.Framework;
using Tests.Framework.Integration;
using Tests.Framework.ManagedElasticsearch.Clusters;

namespace Tests.XPack.MachineLearning.RevertModelSnapshot
{
	public class RevertModelSnapshotApiTests
		: MachineLearningIntegrationTestBase<IRevertModelSnapshotResponse, IRevertModelSnapshotRequest, RevertModelSnapshotDescriptor,
			RevertModelSnapshotRequest>
	{
		public RevertModelSnapshotApiTests(MachineLearningCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override bool ExpectIsValid => true;

		protected override object ExpectJson => new
		{
			delete_intervening_results = true
		};

		protected override int ExpectStatusCode => 200;

		protected override Func<RevertModelSnapshotDescriptor, IRevertModelSnapshotRequest> Fluent => f => f
			.DeleteInterveningResults();

		protected override HttpMethod HttpMethod => HttpMethod.POST;

		protected override RevertModelSnapshotRequest Initializer => new RevertModelSnapshotRequest(CallIsolatedValue, "first")
		{
			DeleteInterveningResults = true
		};

		protected override bool SupportsDeserialization => false;
		protected override string UrlPath => $"/_xpack/ml/anomaly_detectors/{CallIsolatedValue}/model_snapshots/first/_revert";

		protected override void IntegrationSetup(IElasticClient client, CallUniqueValues values)
		{
			foreach (var callUniqueValue in values)
			{
				PutJob(client, callUniqueValue.Value);
				OpenJob(client, callUniqueValue.Value);
				CloseJob(client, callUniqueValue.Value);

				IndexSnapshot(client, callUniqueValue.Value, "first");
				IndexSnapshot(client, callUniqueValue.Value, "second", "2016-06-01T00:00:00Z");

				client.GetModelSnapshots(callUniqueValue.Value).Count.Should().Be(2);
				client.Refresh(".ml-state");
			}
		}

		protected override LazyResponses ClientUsage() => Calls(
			(client, f) => client.RevertModelSnapshot(CallIsolatedValue, "first", f),
			(client, f) => client.RevertModelSnapshotAsync(CallIsolatedValue, "first", f),
			(client, r) => client.RevertModelSnapshot(r),
			(client, r) => client.RevertModelSnapshotAsync(r)
		);

		protected override RevertModelSnapshotDescriptor NewDescriptor() => new RevertModelSnapshotDescriptor(CallIsolatedValue, "first");

		protected override void ExpectResponse(IRevertModelSnapshotResponse response)
		{
			response.Acknowledged.Should().BeTrue();
			response.Model.Should().NotBeNull();
		}
	}
}
