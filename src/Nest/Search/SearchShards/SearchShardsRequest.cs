﻿namespace Nest6
{
	public partial interface ISearchShardsRequest { }

	public partial interface ISearchShardsRequest<T> : ISearchShardsRequest { }

	public partial class SearchShardsRequest { }

	public partial class SearchShardsRequest<T> where T : class { }

	/// <summary>
	/// A descriptor which describes a search operation for _search_shards
	/// </summary>
	public partial class SearchShardsDescriptor<T> where T : class { }
}
