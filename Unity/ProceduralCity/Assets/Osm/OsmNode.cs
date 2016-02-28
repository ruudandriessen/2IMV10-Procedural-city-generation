using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class OsmNode : OsmEntity
	{
		public OsmNode (long id, List<OsmTag> tags, double longitude, double latitude) : base(id, tags)
		{
			this.latitude = latitude;
			this.longitude = longitude;
		}

		private double longitude;
		private double latitude;

		public double getLongitude() {
			return this.longitude;
		}

		public double getLatitude() {
			return this.latitude;
		}
	}
}

