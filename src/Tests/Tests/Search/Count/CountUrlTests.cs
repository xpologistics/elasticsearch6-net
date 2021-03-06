﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest6;
using Tests.Domain;
using Tests.Framework;
using static Tests.Framework.UrlTester;

namespace Tests.Search.Count
{
	public class CountUrlTests
	{
		[U] public async Task Urls()
		{
			var hardcoded = "hardcoded";
			await GET("/devs/developer/_count")
					.Fluent(c => c.Count<Developer>())
					.Request(c => c.Count<Project>(new CountRequest<Developer>()))
					.FluentAsync(c => c.CountAsync<Developer>())
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest<Developer>()))
				;

			await GET("/devs/developer/_count?q=querystring")
					.Fluent(c => c.Count<Developer>(s => s.QueryOnQueryString("querystring")))
					.Request(c => c.Count<Project>(new CountRequest<Developer>() { QueryOnQueryString = "querystring" }))
					.FluentAsync(c => c.CountAsync<Developer>(s => s.QueryOnQueryString("querystring")))
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest<Developer>() { QueryOnQueryString = "querystring" }))
				;
			await GET($"/devs/{hardcoded}/_count")
					.Fluent(c => c.Count<Developer>(s => s.Type(hardcoded)))
					.Request(c => c.Count<Project>(new CountRequest<Developer>(typeof(Developer), hardcoded)))
					.FluentAsync(c => c.CountAsync<Developer>(s => s.Type(hardcoded)))
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest<Developer>(typeof(Developer), hardcoded)))
				;

			await GET("/project/_count")
					.Fluent(c => c.Count<Project>(s => s.Type(Types.All)))
					.Fluent(c => c.Count<Project>(s => s.AllTypes()))
					.Request(c => c.Count<Project>(new CountRequest("project")))
					.Request(c => c.Count<Project>(new CountRequest<Project>("project", Types.All)))
					.FluentAsync(c => c.CountAsync<Project>(s => s.Type(Types.All)))
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest<Project>(typeof(Project), Types.All)))
					.FluentAsync(c => c.CountAsync<Project>(s => s.AllTypes()))
				;

			await GET($"/{hardcoded}/_count")
					.Fluent(c => c.Count<Project>(s => s.Index(hardcoded).Type(Types.All)))
					.Fluent(c => c.Count<Project>(s => s.Index(hardcoded).AllTypes()))
					.Request(c => c.Count<Project>(new CountRequest(hardcoded)))
					.Request(c => c.Count<Project>(new CountRequest<Project>(hardcoded, Types.All)))
					.FluentAsync(c => c.CountAsync<Project>(s => s.Index(hardcoded).Type(Types.All)))
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest<Project>(hardcoded, Types.All)))
					.FluentAsync(c => c.CountAsync<Project>(s => s.Index(hardcoded).AllTypes()))
				;

			await GET("/_count")
					.Fluent(c => c.Count<Project>(s => s.AllTypes().AllIndices()))
					.Request(c => c.Count<Project>(new CountRequest()))
					.Request(c => c.Count<Project>(new CountRequest<Project>(Nest6.Indices.All, Types.All)))
					.FluentAsync(c => c.CountAsync<Project>(s => s.AllIndices().Type(Types.All)))
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest<Project>(Nest6.Indices.All, Types.All)))
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest()))
				;

			await POST("/_count")
					.Fluent(c => c.Count<Project>(s => s.AllTypes().AllIndices().Query(q => q.MatchAll())))
					.Request(c => c.Count<Project>(new CountRequest() { Query = new MatchAllQuery() }))
					.Request(c => c.Count<Project>(new CountRequest<Project>(Nest6.Indices.All, Types.All) { Query = new MatchAllQuery() }))
					.FluentAsync(c => c.CountAsync<Project>(s => s.AllIndices().Type(Types.All).Query(q => q.MatchAll())))
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest<Project>(Nest6.Indices.All, Types.All) { Query = new MatchAllQuery() }))
					.RequestAsync(c => c.CountAsync<Project>(new CountRequest() { Query = new MatchAllQuery() }))
				;
		}
	}
}
