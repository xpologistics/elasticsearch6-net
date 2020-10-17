﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nest6
{
	[JsonObject]
	public interface IWatcherStatsResponse : IResponse
	{
		[JsonProperty("cluster_name")]
		string ClusterName { get; }

		[JsonProperty("manually_stopped")]
		bool ManuallyStopped { get; }

		[JsonProperty("stats")]
		IReadOnlyCollection<WatcherNodeStats> Stats { get; }
	}

	public class WatcherStatsResponse : ResponseBase, IWatcherStatsResponse
	{
		public string ClusterName { get; internal set; }

		public bool ManuallyStopped { get; internal set; }
		public IReadOnlyCollection<WatcherNodeStats> Stats { get; internal set; } = EmptyReadOnly<WatcherNodeStats>.Collection;
	}

	public class WatcherNodeStats
	{
		[JsonProperty("current_watches")]
		public IReadOnlyCollection<WatchRecordStats> CurrentWatches { get; internal set; } = EmptyReadOnly<WatchRecordStats>.Collection;

		[JsonProperty("execution_thread_pool")]
		public ExecutionThreadPool ExecutionThreadPool { get; internal set; }

		[JsonProperty("queued_watches")]
		public IReadOnlyCollection<WatchRecordQueuedStats> QueuedWatches { get; internal set; } = EmptyReadOnly<WatchRecordQueuedStats>.Collection;

		[JsonProperty("watch_count")]
		public long WatchCount { get; internal set; }

		[JsonProperty("watcher_state")]
		public WatcherState WatcherState { get; internal set; }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum WatcherState
	{
		[EnumMember(Value = "stopped")]
		Stopped,

		[EnumMember(Value = "starting")]
		Starting,

		[EnumMember(Value = "started")]
		Started,

		[EnumMember(Value = "stopping")]
		Stopping,
	}

	public class WatchRecordQueuedStats
	{
		[JsonProperty("execution_time")]
		public DateTimeOffset? ExecutionTime { get; internal set; }

		[JsonProperty("triggered_time")]
		public DateTimeOffset? TriggeredTime { get; internal set; }

		[JsonProperty("watch_id")]
		public string WatchId { get; internal set; }

		[JsonProperty("watch_record_id")]
		public string WatchRecordId { get; internal set; }
	}

	public class WatchRecordStats : WatchRecordQueuedStats
	{
		[JsonProperty("execution_phase")]
		public ExecutionPhase? ExecutionPhase { get; internal set; }
	}

	[JsonObject]
	public class ExecutionThreadPool
	{
		[JsonProperty("max_size")]
		public long MaxSize { get; internal set; }

		[JsonProperty("queue_size")]
		public long QueueSize { get; internal set; }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum ExecutionPhase
	{
		[EnumMember(Value = "awaits_execution")]
		AwaitsExecution,

		[EnumMember(Value = "started")]
		Started,

		[EnumMember(Value = "input")]
		Input,

		[EnumMember(Value = "condition")]
		Condition,

		[EnumMember(Value = "actions")]
		Actions,

		[EnumMember(Value = "watch_transform")]
		WatchTransform,

		[EnumMember(Value = "aborted")]
		Aborted,

		[EnumMember(Value = "finished")]
		Finished,
	}
}
