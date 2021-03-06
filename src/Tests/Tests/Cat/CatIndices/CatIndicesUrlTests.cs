﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest6;
using Tests.Domain;
using Tests.Framework;
using static Tests.Framework.UrlTester;

namespace Tests.Cat.CatIndices
{
	public class CatIndicesUrlTests : UrlTestsBase
	{
		[U] public override async Task Urls()
		{
			await GET("/_cat/indices")
					.Fluent(c => c.CatIndices())
					.Request(c => c.CatIndices(new CatIndicesRequest()))
					.FluentAsync(c => c.CatIndicesAsync())
					.RequestAsync(c => c.CatIndicesAsync(new CatIndicesRequest()))
				;

			await GET("/_cat/indices/project")
					.Fluent(c => c.CatIndices(i => i.Index<Project>()))
					.Request(c => c.CatIndices(new CatIndicesRequest(Nest6.Indices.Index<Project>())))
					.FluentAsync(c => c.CatIndicesAsync(i => i.Index<Project>()))
					.RequestAsync(c => c.CatIndicesAsync(new CatIndicesRequest(Nest6.Indices.Index<Project>())))
				;
		}
	}
}
