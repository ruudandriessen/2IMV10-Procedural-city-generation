using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
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

		public Vector2[] getPolygonAsVector2() {
			Vector2[] points = new Vector2[this.getNumberOfNodes()];
			for (int i = 0; i < this.getNumberOfNodes (); i++) {
				OsmNodeReference nodeRef = this.getNodeReference (i);
				Vector2 nodeVector = nodeRef.getVector ();
				nodeVector.Scale (new Vector2 (10000f, 10000f));
				points [i] = nodeVector;
			}
			return points;
		}
	}
}

