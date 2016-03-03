using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class OsmNodeReference
	{
		public OsmNodeReference (long id) {
			this.id = id;
		}

		private long id;
		private float latitude;
		private float longitude;

		public long getId() {
			return this.id;
		}

		public Vector2 getVector() {
			return new Vector2(longitude, latitude);
		}

		public float getLatitude() {
			return this.latitude;
		}

		public float getLongitude() {
			return this.longitude;
		}

		public void setLattitudeAndLongitude(double latitude, double longitude) {
			this.latitude = (float) latitude;
			this.longitude = (float) longitude;
		}
	}
}

