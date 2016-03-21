using System;
using UnityEngine;

namespace ProceduralCity
{
	public class RegionHorizontalBricks : RegionModule
	{
		Color color;
		public RegionHorizontalBricks (Transform parent, Color c)
		{
			this.parent = parent;
			this.setCellDimensions(new Vector3(0.2f, 0.1f, 0.1f));
			this.setCellPadding(new Vector3(0.005f, 0.005f, 0.005f));
			this.color = c;
		}

		#region implemented abstract members of RegionModule

		public override bool apply (Region r)
		{
			Vector3 cornerDimensions = new Vector3 (0.1f, 0.1f, 0.1f);

			HighLevelEdge e1 = r.getEdges ()[0];
			HighLevelEdge e2 = null;
			foreach (HighLevelEdge edge in r.getEdges ()) {
				if (edge.Equals (e1))
					continue;
				if( e1.getFrom().Equals(edge.getFrom()) ) {
					// Same start vertex
					e2 = edge;
					break;
				}
			}
			if (e2 == null) {
				Debug.Log ("Error - no two edges from same start vertex in region");
				return false;
			}
			Vector3 horizontalDir, verticalDir;
			if (e1.getDirection ().y == 0) {
				horizontalDir = e1.getDirection ();
				verticalDir = e2.getDirection ();
			} else {
				horizontalDir = e2.getDirection ();
				verticalDir = e1.getDirection ();
			}
			Corner corner = e1.getFrom ();
			Vector3 p = corner.getVertex().getPoint();
			horizontalDir = parent.TransformVector(horizontalDir);
			verticalDir = parent.TransformVector(verticalDir);
			p = parent.TransformPoint (p);

			// Calculate rotation
			Quaternion rotation = Quaternion.FromToRotation (horizontalDir, Vector3.right);

			float horizontalMagnitude = horizontalDir.magnitude;
			float verticalMagnitude = verticalDir.magnitude;

			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize ();

			horizontalDir = parent.InverseTransformVector (horizontalDir);
			verticalDir = parent.InverseTransformVector (verticalDir);

			Vector3 start = p + Vector3.Scale(corner.getTranslateVector(), cornerDimensions / 2);
			start += cornerDimensions.x / 2 * horizontalDir + dimensions.x / 2 * horizontalDir;
			start += cornerDimensions.y / 2 * verticalDir + dimensions.y / 2 * verticalDir;

			int maxHorizontal = (int) (horizontalMagnitude / dimensions.x);
			int maxVertical = (int) (verticalMagnitude / dimensions.y) - 1;

			for (int i = 0; i < maxHorizontal; i++) {
				for (int j = 0; j < maxVertical; j++) {
					Color finalColor = Color.Lerp (color, Color.black, UnityEngine.Random.value * 0.3f);
					Vector3 location = start + i * dimensions.x * horizontalDir;
//					location += horizontalDir * dimensions.x / 2;
					location += j * dimensions.y * verticalDir;
					Cell c = new Cell (parent, location, scale, rotation, "Region brick");
					c.setColor (finalColor);
				}
			}


			return true;
		}

		#endregion
	}
}

