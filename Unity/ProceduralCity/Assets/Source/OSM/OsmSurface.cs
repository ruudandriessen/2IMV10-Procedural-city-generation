using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public class OsmSurface : OsmWay
	{
		String surfaceType;
		public OsmSurface (long id, List<OsmTag> tags, List<OsmNodeReference> nodes, String type) : base (id, tags, nodes)
		{
			surfaceType = type;
		}

		public String getType() {
			return surfaceType;
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

