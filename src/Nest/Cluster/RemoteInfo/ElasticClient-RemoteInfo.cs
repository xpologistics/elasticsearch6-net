﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Nest6
{
	public partial interface IElasticClient
	{
		/// <summary>
		/// The cluster remote info API allows to retrieve all of the configured remote cluster information.
		/// <para> </para>
		/// <a href="http://www.elastic.co/guide/en/elasticsearch/reference/master/remote-info.html">http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/remote-info.html</a>
		/// </summary>
		/// <param name="selector">An optional descriptor to further describe the remote info operation</param>
		IRemoteInfoResponse RemoteInfo(Func<RemoteInfoDescriptor, IRemoteInfoRequest> selector = null);

		/// <inheritdoc />
		IRemoteInfoResponse RemoteInfo(IRemoteInfoRequest request);

		/// <inheritdoc />
		Task<IRemoteInfoResponse> RemoteInfoAsync(Func<RemoteInfoDescriptor, IRemoteInfoRequest> selector = null,
			CancellationToken cancellationToken = default(CancellationToken)
		);

		/// <inheritdoc />
		Task<IRemoteInfoResponse> RemoteInfoAsync(IRemoteInfoRequest request, CancellationToken cancellationToken = default(CancellationToken));
	}

	public partial class ElasticClient
	{
		/// <inheritdoc />
		public IRemoteInfoResponse RemoteInfo(Func<RemoteInfoDescriptor, IRemoteInfoRequest> selector = null) =>
			RemoteInfo(selector.InvokeOrDefault(new RemoteInfoDescriptor()));

		/// <inheritdoc />
		public IRemoteInfoResponse RemoteInfo(IRemoteInfoRequest request) =>
			Dispatcher.Dispatch<IRemoteInfoRequest, RemoteInfoRequestParameters, RemoteInfoResponse>(
				request,
				(p, d) => LowLevelDispatch.ClusterRemoteInfoDispatch<RemoteInfoResponse>(p)
			);

		/// <inheritdoc />
		public Task<IRemoteInfoResponse> RemoteInfoAsync(Func<RemoteInfoDescriptor, IRemoteInfoRequest> selector = null,
			CancellationToken cancellationToken = default(CancellationToken)
		) =>
			RemoteInfoAsync(selector.InvokeOrDefault(new RemoteInfoDescriptor()), cancellationToken);

		/// <inheritdoc />
		public Task<IRemoteInfoResponse> RemoteInfoAsync(IRemoteInfoRequest request, CancellationToken cancellationToken = default(CancellationToken)
		) =>
			Dispatcher.DispatchAsync<IRemoteInfoRequest, RemoteInfoRequestParameters, RemoteInfoResponse, IRemoteInfoResponse>(
				request,
				cancellationToken,
				(p, d, c) => LowLevelDispatch.ClusterRemoteInfoDispatchAsync<RemoteInfoResponse>(p, c)
			);
	}
}
