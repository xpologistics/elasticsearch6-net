﻿using System.IO;
using System.Linq;
using Elastic.Xunit.XunitPlumbing;
using Elasticsearch.Net;
using FluentAssertions;
using Nest6;
using Tests.Core.Client;
using Tests.Framework;

namespace Tests.Document.Multiple.Bulk
{
	public class BulkResponseParstingTests : DocumentationTestBase
	{
		[U] public void CanDeserialize()
		{
			var client = TestClient.DefaultInMemoryClient;
			var count = 100000;
			var bytes = client.RequestResponseSerializer.SerializeToBytes(ReturnBulkResponse(count));
			var x = Deserialize(bytes, client);
			x.Items.Should().HaveCount(count).And.NotContain(i => i == null);
		}

		private BulkResponse Deserialize(byte[] response, IElasticClient client)
		{
			using (var ms = new MemoryStream(response))
				return client.RequestResponseSerializer.Deserialize<BulkResponse>(ms);
		}

		private static object BulkItemResponse() => new
		{
			index = new
			{
				_index = "nest-52cfd7aa",
				_type = "project",
				_id = "Kuhn LLC",
				_version = 1,
				_shards = new
				{
					total = 2,
					successful = 1,
					failed = 0
				},
				created = true,
				status = 201
			}
		};


		private static object ReturnBulkResponse(int numberOfItems) => new
		{
			took = 276,
			errors = false,
			items = Enumerable.Range(0, numberOfItems)
				.Select(i => BulkItemResponse())
				.ToArray()
		};
	}
}
