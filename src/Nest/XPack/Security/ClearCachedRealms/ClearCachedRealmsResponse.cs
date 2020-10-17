﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nest6
{
	public interface IClearCachedRealmsResponse : IResponse
	{
		[JsonProperty("cluster_name")]
		string ClusterName { get; }

		[JsonProperty("nodes")]
		IReadOnlyDictionary<string, SecurityNode> Nodes { get; }
	}

	public class ClearCachedRealmsResponse : ResponseBase, IClearCachedRealmsResponse
	{
		public string ClusterName { get; internal set; }
		public IReadOnlyDictionary<string, SecurityNode> Nodes { get; internal set; } = EmptyReadOnly<string, SecurityNode>.Dictionary;
	}
}
