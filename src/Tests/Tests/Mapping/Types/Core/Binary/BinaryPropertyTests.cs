﻿using System;
using Nest6;
using Tests.Core.ManagedElasticsearch.Clusters;
using Tests.Domain;
using Tests.Framework.Integration;

namespace Tests.Mapping.Types.Core.Binary
{
	public class BinaryPropertyTests : PropertyTestsBase
	{
		public BinaryPropertyTests(WritableCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override object ExpectJson => new
		{
			properties = new
			{
				name = new
				{
					type = "binary",
					doc_values = true,
					similarity = "classic",
					store = true
				}
			}
		};

		protected override Func<PropertiesDescriptor<Project>, IPromise<IProperties>> FluentProperties => f => f
			.Binary(b => b
				.Name(p => p.Name)
				.DocValues()
				.Similarity(SimilarityOption.Classic)
				.Store()
			);

		protected override IProperties InitializerProperties => new Properties
		{
			{
				"name", new BinaryProperty
				{
					DocValues = true,
					Similarity = SimilarityOption.Classic,
					Store = true
				}
			}
		};
	}
}
