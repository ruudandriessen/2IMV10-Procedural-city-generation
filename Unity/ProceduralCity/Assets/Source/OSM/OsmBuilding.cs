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
				points [i + 1] = nodeRef.getLattitude() *10000f;
			}
			return points;
		}



	}
}

