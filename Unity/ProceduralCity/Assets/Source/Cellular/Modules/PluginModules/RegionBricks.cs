using System;
using UnityEngine;

namespace ProceduralCity
{
	public class RegionBricks : RegionModule
	{
		Color color;
		public RegionBricks (Transform parent, Color c)
		{
			this.parent = parent;
			this.setCellDimensions(new Vector3(0.1f, 0.1f, 0.2f));
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

			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize ();

			float horizontalMagnitude = horizontalDir.magnitude - dimensions.z;
			float verticalMagnitude = verticalDir.magnitude;

			horizontalDir = parent.InverseTransformVector (horizontalDir);
			verticalDir = parent.InverseTransformVector (verticalDir);

			Vector3 start = p + Vector3.Scale(corner.getTranslateVector(), cornerDimensions / 2);
			start += cornerDimensions.z / 2 * horizontalDir;// + dimensions.z / 2 * horizontalDir;
			start += cornerDimensions.y / 2 * verticalDir + dimensions.y / 2 * verticalDir;

			bool brickOut = horizontalDir.x == 0;

			int maxHorizontal = (int) (horizontalMagnitude / dimensions.z);
			int maxVertical = (int) (verticalMagnitude / dimensions.y) - 1;

			for (int i = 0; i < maxVertical; i++) {
				Vector3 rowStart = start + i * dimensions.y * verticalDir;
				Vector3 rowEnd = rowStart + horizontalMagnitude * horizontalDir;
				if (brickOut) {
					rowStart += dimensions.z / 2 * horizontalDir;
					rowEnd -= dimensions.z / 2 * horizontalDir;
				}
//				Debug.DrawLine (rowStart, rowEnd, Color.green, 200);
				FillCellModule.fillCell (rowStart, rowEnd, scale, dimensions, parent);
				brickOut = !brickOut;
			}
			return true;
		}

		#endregion
	}
}

