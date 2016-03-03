using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public class OsmBuilding : OsmWay
	{
		public OsmBuilding (long id, List<OsmTag> tags, List<OsmNodeReference> nodes) : base (id, tags, nodes)
		{
		}

		public float[] getPolygon() {
			float[] points = new float[this.getNumberOfNodes()*2];
			for (int i = 0; i < this.getNumberOfNodes (); i++) {
				OsmNodeReference nodeRef = this.getNodeReference (i);
				points [i] = nodeRef.getLongitude() *10000f;
				points [i + 1] = nodeRef.getLatitude() *10000f;
			}
			return points;
		}

		public Vector3[] getPolygonAsVector3() {
			Vector3[] points = new Vector3[this.getNumberOfNodes()-1];
			for (int i = 0; i < this.getNumberOfNodes ()-1; i++) {
				OsmNodeReference nodeRef = this.getNodeReference (i);
				float lat = nodeRef.getLatitude();
				float lon = nodeRef.getLongitude();
				points [i] = Map.getVectorFromOrigin (lat, lon);
			}
			return points;
		}

	}
}

