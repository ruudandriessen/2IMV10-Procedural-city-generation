using System;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class OsmNode : OsmEntity
	{
		public OsmNode (long id, List<OsmTag> tags, float longitude, float latitude) : base(id, tags)
		{
			this.latitude = latitude;
			this.longitude = longitude;
			this.containedIn = new List<OsmWay> ();
		}

		private float longitude;
		private float latitude;
		private List<OsmWay> containedIn;

		public List<OsmWay> getContainedIn() {
			return containedIn;
		}

		public OsmWay getContainedIn(int index) {
			return containedIn [index];
		}

		public void addContainedIn(OsmWay way) {
			containedIn.Add (way);
		}

		public int amountContainedIn() {
			return containedIn.Count;
		}


		public float getLongitude() {
			return this.longitude;
		}

		public float getLatitude() {
			return this.latitude;
		}
	}
}

