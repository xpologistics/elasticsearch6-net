﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest6;
using Tests.Domain;
using Tests.Framework;
using static Tests.Framework.UrlTester;

namespace Tests.Cat.CatShards
{
	public class CatShardsUrlTests : UrlTestsBase
	{
		[U] public override async Task Urls()
		{
			await GET("/_cat/shards")
					.Fluent(c => c.CatShards())
					.Request(c => c.CatShards(new CatShardsRequest()))
					.FluentAsync(c => c.CatShardsAsync())
					.RequestAsync(c => c.CatShardsAsync(new CatShardsRequest()))
				;

			await GET("/_cat/shards/project")
				.Fluent(c => c.CatShards(r => r.Index<Project>()))
				.Request(c => c.CatShards(new CatShardsRequest(Nest.Indices.Index<Project>())))
				.FluentAsync(c => c.CatShardsAsync(r => r.Index<Project>()))
				.RequestAsync(c => c.CatShardsAsync(new CatShardsRequest(Nest.Indices.Index<Project>())));
		}
	}
}
