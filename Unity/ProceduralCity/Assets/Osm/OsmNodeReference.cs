using System;
using UnityEngine;

namespace ProceduralCity
{
	public class OsmNodeReference
	{
		public OsmNodeReference (long id) {
			this.id = id;
		}

		private long id;
		private float lattitude;
		private float longitude;

		public long getId() {
			return this.id;
		}

		public Vector2 getVector() {
			return new Vector2(longitude, lattitude);
		}

		public float getLattitude() {
			return this.lattitude;
		}

		public float getLongitude() {
			return this.longitude;
		}

		public void setLattitudeAndLongitude(double lattitude, double longitude) {
			this.lattitude = (float) lattitude;
			this.longitude = (float) longitude;
		}
	}
}

