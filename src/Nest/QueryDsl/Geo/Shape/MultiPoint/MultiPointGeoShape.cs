﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nest6
{
	public interface IMultiPointGeoShape : IGeoShape
	{
		[JsonProperty("coordinates")]
		IEnumerable<GeoCoordinate> Coordinates { get; set; }
	}

	public class MultiPointGeoShape : GeoShapeBase, IMultiPointGeoShape
	{
		public MultiPointGeoShape() : this(null) { }

		public MultiPointGeoShape(IEnumerable<GeoCoordinate> coordinates)
			: base("multipoint") => Coordinates = coordinates;

		public IEnumerable<GeoCoordinate> Coordinates { get; set; }
	}
}
