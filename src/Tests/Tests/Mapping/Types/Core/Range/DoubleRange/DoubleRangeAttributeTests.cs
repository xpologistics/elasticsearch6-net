﻿using Elastic.Xunit.XunitPlumbing;
using Nest6;

namespace Tests.Mapping.Types.Core.Range.DoubleRange
{
	public class DoubleRangeTest
	{
		[DoubleRange]
		public Nest6.DoubleRange Range { get; set; }
	}

	[SkipVersion("<5.2.0", "dedicated range types is a new 5.2.0 feature")]
	public class DoubleRangeAttributeTests : AttributeTestsBase<DoubleRangeTest>
	{
		protected override object ExpectJson => new
		{
			properties = new
			{
				range = new
				{
					type = "double_range"
				}
			}
		};
	}
}
