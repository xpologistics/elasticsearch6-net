﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest6;
using Tests.Framework;
using static Tests.Framework.UrlTester;

namespace Tests.Cat.CatPendingTasks
{
	public class CatPendingTasksUrlTests : UrlTestsBase
	{
		[U] public override async Task Urls() => await GET("/_cat/pending_tasks")
			.Fluent(c => c.CatPendingTasks())
			.Request(c => c.CatPendingTasks(new CatPendingTasksRequest()))
			.FluentAsync(c => c.CatPendingTasksAsync())
			.RequestAsync(c => c.CatPendingTasksAsync(new CatPendingTasksRequest()));
	}
}
