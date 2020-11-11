﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch6.Net;

namespace Nest6
{
	public partial interface IElasticClient
	{
		/// <summary>
		/// The get settings API allows to retrieve settings of index/indices.
		/// <para> </para>
		/// http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-get-settings.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the get index settings operation</param>
		IGetIndexSettingsResponse GetIndexSettings(Func<GetIndexSettingsDescriptor, IGetIndexSettingsRequest> selector);

		/// <inheritdoc />
		IGetIndexSettingsResponse GetIndexSettings(IGetIndexSettingsRequest request);

		/// <inheritdoc />
		Task<IGetIndexSettingsResponse> GetIndexSettingsAsync(Func<GetIndexSettingsDescriptor, IGetIndexSettingsRequest> selector,
			CancellationToken cancellationToken = default(CancellationToken)
		);

		/// <inheritdoc />
		Task<IGetIndexSettingsResponse> GetIndexSettingsAsync(IGetIndexSettingsRequest request,
			CancellationToken cancellationToken = default(CancellationToken)
		);
	}

	public partial class ElasticClient
	{
		/// <inheritdoc />
		public IGetIndexSettingsResponse GetIndexSettings(Func<GetIndexSettingsDescriptor, IGetIndexSettingsRequest> selector) =>
			GetIndexSettings(selector?.Invoke(new GetIndexSettingsDescriptor()));

		/// <inheritdoc />
		public IGetIndexSettingsResponse GetIndexSettings(IGetIndexSettingsRequest request) =>
			Dispatcher.Dispatch<IGetIndexSettingsRequest, GetIndexSettingsRequestParameters, GetIndexSettingsResponse>(
				request,
				(p, d) => LowLevelDispatch.IndicesGetSettingsDispatch<GetIndexSettingsResponse>(p)
			);

		/// <inheritdoc />
		public Task<IGetIndexSettingsResponse> GetIndexSettingsAsync(Func<GetIndexSettingsDescriptor, IGetIndexSettingsRequest> selector,
			CancellationToken cancellationToken = default(CancellationToken)
		) =>
			GetIndexSettingsAsync(selector?.Invoke(new GetIndexSettingsDescriptor()), cancellationToken);

		/// <inheritdoc />
		public Task<IGetIndexSettingsResponse> GetIndexSettingsAsync(IGetIndexSettingsRequest request,
			CancellationToken cancellationToken = default(CancellationToken)
		) =>
			Dispatcher
				.DispatchAsync<IGetIndexSettingsRequest, GetIndexSettingsRequestParameters, GetIndexSettingsResponse, IGetIndexSettingsResponse>(
					request,
					cancellationToken,
					(p, d, c) => LowLevelDispatch.IndicesGetSettingsDispatchAsync<GetIndexSettingsResponse>(p, c)
				);
	}
}
