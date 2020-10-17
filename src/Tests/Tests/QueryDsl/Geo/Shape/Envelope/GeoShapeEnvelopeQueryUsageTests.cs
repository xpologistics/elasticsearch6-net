﻿using System.Collections.Generic;
using Nest6;
using Tests.Core.ManagedElasticsearch.Clusters;
using Tests.Domain;
using Tests.Framework.Integration;
using static Nest6.Infer;

namespace Tests.QueryDsl.Geo.Shape.Envelope
{
	public class GeoShapeEnvelopeQueryUsageTests : GeoShapeQueryUsageTestsBase
	{
		public GeoShapeEnvelopeQueryUsageTests(ReadOnlyCluster i, EndpointUsage usage) : base(i, usage) { }

		protected override ConditionlessWhen ConditionlessWhen =>
			new ConditionlessWhen<IGeoShapeEnvelopeQuery>(a => a.GeoShape as IGeoShapeEnvelopeQuery)
			{
				q => q.Field = null,
				q => q.Shape = null,
				q => q.Shape.Coordinates = null,
			};

		protected override QueryContainer QueryInitializer => new GeoShapeEnvelopeQuery
		{
			Name = "named_query",
			Boost = 1.1,
			Field = Field<Project>(p => p.LocationShape),
			Shape = new EnvelopeGeoShape(EnvelopeCoordinates),
			Relation = GeoShapeRelation.Intersects,
		};

		protected override object ShapeJson => new
		{
			type = "envelope",
			coordinates = EnvelopeCoordinates
		};

		protected override QueryContainer QueryFluent(QueryContainerDescriptor<Project> q) => q
			.GeoShapeEnvelope(c => c
				.Name("named_query")
				.Boost(1.1)
				.Field(p => p.LocationShape)
				.Coordinates(EnvelopeCoordinates)
				.Relation(GeoShapeRelation.Intersects)
			);
	}
}
