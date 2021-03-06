﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nest6
{
	/// <summary>
	/// A PutWatch request
	/// </summary>
	public partial interface IPutWatchRequest
	{
		/// <summary>
		/// The actions that will be run if the condition matches
		/// </summary>
		[JsonProperty("actions")]
		Actions Actions { get; set; }

		/// <summary>
		/// Defines if the actions should be run
		/// </summary>
		[JsonProperty("condition")]
		ConditionContainer Condition { get; set; }

		/// <summary>
		/// Defines the input that loads the data for the watch
		/// </summary>
		[JsonProperty("input")]
		InputContainer Input { get; set; }

		/// <summary>
		/// Metadata that will be copied into the history entries
		/// </summary>
		[JsonProperty("metadata")]
		[JsonConverter(typeof(VerbatimDictionaryKeysJsonConverter<string, object>))]
		IDictionary<string, object> Metadata { get; set; }

		/// <summary>
		/// The minimum time between actions being run. Defaults to 5 seconds.
		/// </summary>
		/// <remarks>
		/// Default can be changed in the config file with the setting <code>xpack.watcher.throttle.period.default_period</code>.
		/// </remarks>
		[JsonProperty("throttle_period")]
		string ThrottlePeriod { get; set; }

		/// <summary>
		/// Processes and changes the payload in the watch execution context to prepare it for the actions.
		/// </summary>
		[JsonProperty("transform")]
		TransformContainer Transform { get; set; }

		/// <summary>
		/// Defines when the watch should run
		/// </summary>
		[JsonProperty("trigger")]
		TriggerContainer Trigger { get; set; }
	}

	/// <inheritdoc cref="IPutWatchRequest" />
	public partial class PutWatchRequest
	{
		public PutWatchRequest() { }

		/// <inheritdoc />
		public Actions Actions { get; set; }

		/// <inheritdoc />
		public ConditionContainer Condition { get; set; }

		/// <inheritdoc />
		public InputContainer Input { get; set; }

		/// <inheritdoc />
		public IDictionary<string, object> Metadata { get; set; }

		/// <inheritdoc />
		public string ThrottlePeriod { get; set; }

		/// <inheritdoc />
		public TransformContainer Transform { get; set; }

		/// <inheritdoc />
		public TriggerContainer Trigger { get; set; }
	}

	/// <inheritdoc cref="IPutWatchRequest" />
	[DescriptorFor("XpackWatcherPutWatch")]
	public partial class PutWatchDescriptor
	{
		public PutWatchDescriptor() { }

		Actions IPutWatchRequest.Actions { get; set; }
		ConditionContainer IPutWatchRequest.Condition { get; set; }
		InputContainer IPutWatchRequest.Input { get; set; }
		IDictionary<string, object> IPutWatchRequest.Metadata { get; set; }
		string IPutWatchRequest.ThrottlePeriod { get; set; }
		TransformContainer IPutWatchRequest.Transform { get; set; }
		TriggerContainer IPutWatchRequest.Trigger { get; set; }

		/// <inheritdoc cref="IPutWatchRequest.Actions" />
		public PutWatchDescriptor Actions(Func<ActionsDescriptor, IPromise<Actions>> actions) =>
			Assign(actions, (a, v) => a.Actions = v?.Invoke(new ActionsDescriptor())?.Value);

		/// <inheritdoc cref="IPutWatchRequest.Condition" />
		public PutWatchDescriptor Condition(Func<ConditionDescriptor, ConditionContainer> selector) =>
			Assign(selector.InvokeOrDefault(new ConditionDescriptor()), (a, v) => a.Condition = v);

		/// <inheritdoc cref="IPutWatchRequest.Input" />
		public PutWatchDescriptor Input(Func<InputDescriptor, InputContainer> selector) =>
			Assign(selector.InvokeOrDefault(new InputDescriptor()), (a, v) => a.Input = v);

		/// <inheritdoc cref="IPutWatchRequest.Metadata" />
		public PutWatchDescriptor Metadata(Func<FluentDictionary<string, object>, FluentDictionary<string, object>> paramsDictionary) =>
			Assign(paramsDictionary, (a, v) => a.Metadata = v?.Invoke(new FluentDictionary<string, object>()));

		/// <inheritdoc cref="IPutWatchRequest.Metadata" />
		public PutWatchDescriptor Metadata(Dictionary<string, object> paramsDictionary) =>
			Assign(paramsDictionary, (a, v) => a.Metadata = v);

		/// <inheritdoc cref="IPutWatchRequest.ThrottlePeriod" />
		public PutWatchDescriptor ThrottlePeriod(string throttlePeriod) => Assign(throttlePeriod, (a, v) => a.ThrottlePeriod = v);

		/// <inheritdoc cref="IPutWatchRequest.Transform" />
		public PutWatchDescriptor Transform(Func<TransformDescriptor, TransformContainer> selector) =>
			Assign(selector.InvokeOrDefault(new TransformDescriptor()), (a, v) => a.Transform = v);

		/// <inheritdoc cref="IPutWatchRequest.Trigger" />
		public PutWatchDescriptor Trigger(Func<TriggerDescriptor, TriggerContainer> selector) =>
			Assign(selector.InvokeOrDefault(new TriggerDescriptor()), (a, v) => a.Trigger = v);
	}
}
