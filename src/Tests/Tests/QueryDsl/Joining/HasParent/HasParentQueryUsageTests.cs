﻿using Nest6;
using Tests.Core.ManagedElasticsearch.Clusters;
using Tests.Domain;
using Tests.Framework.Integration;

namespace Tests.QueryDsl.Joining.HasParent
{
	public class HasParentUsageTests : QueryDslUsageTestsBase
	{
		public HasParentUsageTests(ReadOnlyCluster i, EndpointUsage usage) : base(i, usage) { }

		protected override ConditionlessWhen ConditionlessWhen => new ConditionlessWhen<IHasParentQuery>(a => a.HasParent)
		{
			q => q.Query = null,
			q => q.Query = ConditionlessQuery,
			q => q.ParentType = null,
		};

		protected override QueryContainer QueryInitializer => new HasParentQuery
		{
			Name = "named_query",
			Boost = 1.1,
			ParentType = Infer.Type<Developer>(),
			InnerHits = new InnerHits { Explain = true },
			Query = new MatchAllQuery(),
			Score = true,
			IgnoreUnmapped = true
		};

		protected override object QueryJson => new
		{
			has_parent = new
			{
				_name = "named_query",
				boost = 1.1,
				parent_type = "developer",
				score = true,
				ignore_unmapped = true,
				query = new
				{
					match_all = new { }
				},
				inner_hits = new
				{
					explain = true
				}
			}
		};

		protected override QueryContainer QueryFluent(QueryContainerDescriptor<Project> q) => q
			.HasParent<Developer>(c => c
				.Name("named_query")
				.Boost(1.1)
				.InnerHits(i => i.Explain())
				.Score(true)
				.Query(qq => qq.MatchAll())
				.IgnoreUnmapped(true)
			);
	}
}
