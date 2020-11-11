﻿namespace Elasticsearch6.Net
{
	public class DynamicResponse : ElasticsearchResponse<dynamic>
	{
		public DynamicResponse() { }

		public DynamicResponse(DynamicBody body) => Body = body;
	}
}
