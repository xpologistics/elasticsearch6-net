﻿using System;
using System.Text;
using Elasticsearch.Net;
using Nest6;

namespace Tests.Core.Client
{
	public static class FixedResponseClient
	{
		public static IElasticClient Create(
			object response,
			int statusCode = 200,
			Func<ConnectionSettings, ConnectionSettings> modifySettings = null,
			string contentType = RequestData.MimeType,
			Exception exception = null
		)
		{
			var settings = CreateConnectionSettings(response, statusCode, modifySettings, contentType, exception);
			return new ElasticClient(settings);
		}

		public static ConnectionSettings CreateConnectionSettings(
			object response,
			int statusCode = 200,
			Func<ConnectionSettings, ConnectionSettings> modifySettings = null,
			string contentType = RequestData.MimeType,
			Exception exception = null
		)
		{
			var serializer = TestClient.Default.RequestResponseSerializer;
			byte[] responseBytes;
			switch (response)
			{
				case string s:
					responseBytes = Encoding.UTF8.GetBytes(s);
					break;
				case byte[] b:
					responseBytes = b;
					break;
				default:
				{
					responseBytes = contentType == RequestData.MimeType
						? serializer.SerializeToBytes(response)
						: Encoding.UTF8.GetBytes(response.ToString());
					break;
				}
			}

			var connection = new InMemoryConnection(responseBytes, statusCode, exception, contentType);
			var connectionPool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
			var defaultSettings = new ConnectionSettings(connectionPool, connection)
				.DefaultIndex("default-index");
			var settings = modifySettings != null ? modifySettings(defaultSettings) : defaultSettings;
			return settings;
		}
	}
}
