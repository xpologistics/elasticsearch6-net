﻿using Elastic.Xunit.XunitPlumbing;
using Nest6;

namespace Tests.Mapping.Types.Core.Range.LongRange
{
	public class LongRangeTest
	{
		[LongRange]
		public Nest6.LongRange Range { get; set; }
	}

	[SkipVersion("<5.2.0", "dedicated range types is a new 5.2.0 feature")]
	public class LongRangeAttributeTests : AttributeTestsBase<LongRangeTest>
	{
		protected override object ExpectJson => new
		{
			properties = new
			{
				range = new
				{
					type = "long_range"
				}
			}
		};
	}
}
