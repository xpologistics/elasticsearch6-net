using System;
using System.Threading.Tasks;
using Nest6;
using Tests.Domain;
using Tests.Profiling.Framework;
using Tests.Profiling.Framework.Performance;

namespace Tests.Profiling
{
	public class BulkProfileTests
	{
		private static readonly string IndexName = "bulk-profile";
		private readonly IElasticClient _client;

		public BulkProfileTests(ProfilingCluster cluster)
		{
			_client = cluster.Client;

			if (_client.IndexExists(IndexName).Exists)
				_client.DeleteIndex(IndexName);

			var createIndexResponse = _client.CreateIndex(IndexName);

			if (!createIndexResponse.IsValid)
				Console.WriteLine($"invalid response creating index: {createIndexResponse.ServerError?.Error?.Reason}");
		}

		[Performance(Iterations = 10)]
		public void Sync()
		{
			var bulkResponse = _client.Bulk(b => b
				.IndexMany(Developer.Generator.Generate(1000), (bd, d) => bd
					.Index(IndexName)
					.Document(d)
				));

			if (!bulkResponse.IsValid)
			{
				if (bulkResponse.Errors)
					foreach (var error in bulkResponse.ItemsWithErrors)
						Console.WriteLine($"error with id {error.Id}. message: {error.Error.Reason}");
			}
		}

		[Performance(Iterations = 10)]
		public async Task Async()
		{
			var bulkResponse = await _client.BulkAsync(b => b
					.IndexMany(Developer.Generator.Generate(1000), (bd, d) => bd
						.Index(IndexName)
						.Document(d)
					))
				.ConfigureAwait(false);

			if (!bulkResponse.IsValid)
			{
				if (bulkResponse.Errors)
					foreach (var error in bulkResponse.ItemsWithErrors)
						Console.WriteLine($"error with id {error.Id}. message: {error.Error.Reason}");
			}
		}
	}
}
