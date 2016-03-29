using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class RegionBricks : RegionModule
	{
		Color color;
		public RegionBricks (Transform parent, Color c)
		{
			this.parent = parent;
			this.setCellDimensions(new Vector3(1.0f, 1.0f, 4.0f));
			this.setCellPadding(new Vector3(0.05f, 0.05f, 0.05f));
			this.color = c;
		}

		#region implemented abstract members of RegionModule

		public override List<MeshFilter> apply (Region r)
		{
			List<MeshFilter> meshes = new List<MeshFilter> ();
			Vector3 cornerDimensions = new Vector3 (1.0f, 1.0f, 1.0f);

			HighLevelEdge e1 = null;
			HighLevelEdge e2 = null;
			foreach (HighLevelEdge edgeFirst in r.getEdges()) {
				e1 = edgeFirst;
				foreach (HighLevelEdge edgeSecond in r.getEdges ()) {
					if (edgeSecond.Equals (e1))
						continue;
					if (
						(e1.getFrom ().Equals (edgeSecond.getFrom ()) || e1.getFrom ().Equals (edgeSecond.getTo ()))
						&& e1.getDirection ().y != edgeSecond.getDirection ().y) {
						// Same start vertex
						e2 = edgeSecond;
						break;
					}
				}
				if (e2 != null)
					break;
			}
			if (e2 == null) {
				Debug.Log ("Error - no two edges from same start vertex in region with different y values");
				return meshes;
			}
			Vector3 horizontalDir, verticalDir;
			if (e1.getDirection ().y == 0) {
				horizontalDir = e1.getDirection (e1.getFrom());
				verticalDir = e2.getDirection (e1.getFrom());
			} else {
				horizontalDir = e2.getDirection (e1.getFrom());
				verticalDir = e1.getDirection (e1.getFrom());
			}
			Corner corner = e1.getFrom ();
			Vector3 p = corner.getVertex().getPoint();
			horizontalDir = parent.TransformVector(horizontalDir);
			verticalDir = parent.TransformVector(verticalDir);
			p = parent.TransformPoint (p);

			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize ();

			float horizontalMagnitude = horizontalDir.magnitude - 3.0f;
			float verticalMagnitude = verticalDir.magnitude;

			horizontalDir = parent.InverseTransformVector (horizontalDir);
			verticalDir = parent.InverseTransformVector (verticalDir);

			verticalDir.Normalize ();
			horizontalDir.Normalize ();

			Vector3 start = p + Vector3.Scale(corner.getTranslateVector(), cornerDimensions / 2);
			start += cornerDimensions.z / 2 * horizontalDir;// + dimensions.z / 2 * horizontalDir;
			start += cornerDimensions.y / 2 * verticalDir + dimensions.y / 2 * verticalDir;

			bool brickOut = verticalDir.y < 0;

			int maxVertical = (int) (verticalMagnitude / dimensions.y) - 2;

			for (int i = 0; i < maxVertical; i++) {
				Vector3 rowStart = start + i * dimensions.y * verticalDir;
				Vector3 rowEnd = rowStart + horizontalMagnitude * horizontalDir;
				if (brickOut) {
					rowStart += 2.0f / 2 * horizontalDir;
					rowEnd += 2.0f / 2 * horizontalDir;
				}
				Vector3 normal = r.getNormal ();
				meshes.AddRange(FillCellModule.fillCell (rowStart, rowEnd, scale, dimensions, parent, color, normal));
				brickOut = !brickOut;
			}


			return meshes;
		}

		#endregion
	}
}

