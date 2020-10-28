﻿using System;
using Elastic.Xunit.XunitPlumbing;
using Elasticsearch.Net;
using FluentAssertions;
using Nest6;
using Tests.Framework;

namespace Tests.CodeStandards.Serialization
{
	public class JsonProperties
	{
		/**
		* Our Json.NET contract resolver picks up attributes set on the interface
		*/
		[U]
		public void SeesInterfaceProperties()
		{
			var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
			var settings = new ConnectionSettings(pool, new InMemoryConnection());
			var c = new ElasticClient(settings);


			var serializer = c.RequestResponseSerializer;
			var serialized = serializer.SerializeToString(new Nest6.Analysis { CharFilters = new CharFilters() });
			serialized.Should().NotContain("char_filters").And.NotContain("charFilters");
			serialized.Should().Contain("char_filter");

			serialized = serializer.SerializeToString(new AnalysisDescriptor().CharFilters(cf=>cf));
			serialized.Should().NotContain("char_filters").And.NotContain("charFilters");
			serialized.Should().Contain("char_filter");
		}
	}
}
