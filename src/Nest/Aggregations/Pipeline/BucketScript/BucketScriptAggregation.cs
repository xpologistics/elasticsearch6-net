﻿using System;
using Newtonsoft.Json;

namespace Nest6
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[ContractJsonConverter(typeof(AggregationJsonConverter<BucketScriptAggregation>))]
	public interface IBucketScriptAggregation : IPipelineAggregation
	{
		[JsonProperty("script")]
		IScript Script { get; set; }
	}

	public class BucketScriptAggregation
		: PipelineAggregationBase, IBucketScriptAggregation
	{
		internal BucketScriptAggregation() { }

		public BucketScriptAggregation(string name, MultiBucketsPath bucketsPath)
			: base(name, bucketsPath) { }

		public IScript Script { get; set; }

		internal override void WrapInContainer(AggregationContainer c) => c.BucketScript = this;
	}

	public class BucketScriptAggregationDescriptor
		: PipelineAggregationDescriptorBase<BucketScriptAggregationDescriptor, IBucketScriptAggregation, MultiBucketsPath>
			, IBucketScriptAggregation
	{
		IScript IBucketScriptAggregation.Script { get; set; }

		public BucketScriptAggregationDescriptor Script(string script) => Assign((InlineScript)script, (a, v) => a.Script = v);

		public BucketScriptAggregationDescriptor Script(Func<ScriptDescriptor, IScript> scriptSelector) =>
			Assign(scriptSelector, (a, v) => a.Script = v?.Invoke(new ScriptDescriptor()));

		public BucketScriptAggregationDescriptor BucketsPath(Func<MultiBucketsPathDescriptor, IPromise<IBucketsPath>> selector) =>
			Assign(selector, (a, v) => a.BucketsPath = v?.Invoke(new MultiBucketsPathDescriptor())?.Value);
	}
}
