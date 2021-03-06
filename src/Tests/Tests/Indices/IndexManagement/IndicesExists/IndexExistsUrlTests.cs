﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest6;
using Tests.Domain;
using Tests.Framework;
using static Nest6.Indices;
using static Tests.Framework.UrlTester;

namespace Tests.Indices.IndexManagement.IndexExists
{
	public class IndexExistsUrlTests
	{
		[U] public async Task Urls()
		{
			var indices = Index<Project>().And<CommitActivity>();
			var index = "project";
			await HEAD($"/{index}")
					.Fluent(c => c.IndexExists(index, s => s))
					.Request(c => c.IndexExists(new IndexExistsRequest(index)))
					.FluentAsync(c => c.IndexExistsAsync(index))
					.RequestAsync(c => c.IndexExistsAsync(new IndexExistsRequest(index)))
				;
		}
	}
}
