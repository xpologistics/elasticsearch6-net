﻿namespace Nest6
{
	public class Murmur3HashAttribute : ElasticsearchDocValuesPropertyAttributeBase, IMurmur3HashProperty
	{
		public Murmur3HashAttribute() : base(FieldType.Murmur3Hash) { }
	}
}
