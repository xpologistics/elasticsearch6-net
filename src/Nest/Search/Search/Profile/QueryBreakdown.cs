using Newtonsoft.Json;

namespace Nest6
{
	public class QueryBreakdown
	{
		[JsonProperty("advance")]
		public long Advance { get; internal set; }

		[JsonProperty("build_scorer")]
		public long BuildScorer { get; internal set; }

		[JsonProperty("create_weight")]
		public long CreateWeight { get; internal set; }

		[JsonProperty("match")]
		public long Match { get; internal set; }

		[JsonProperty("next_doc")]
		public long NextDoc { get; internal set; }

		[JsonProperty("score")]
		public long Score { get; internal set; }
	}
}
