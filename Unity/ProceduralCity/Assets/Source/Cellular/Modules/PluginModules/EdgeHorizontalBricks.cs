using System;
using UnityEngine;

namespace ProceduralCity
{
	public class EdgeHorizontalBricks : EdgeModule
	{
		Color color;
		public EdgeHorizontalBricks (Transform parent, Color c)
		{
			this.parent = parent;
			this.setCellDimensions(new Vector3(1.0f, 1.0f, 2.0f));
			this.setCellPadding(new Vector3(0.05f, 0.05f, 0.05f));
			this.color = c;
		}

		#region implemented abstract members of EdgeModule

		public override bool apply (HighLevelEdge edge)
		{
			Vector3 cornerDimensions = new Vector3 (1.0f, 1.0f, 1.0f);

			// Get points of edge and translate to world space
			Corner cFrom = edge.getFrom();
			Vector3 from = edge.getFrom ().getVertex().getPoint();
			Vector3 to = edge.getTo ().getVertex().getPoint();
			from = parent.TransformPoint (from);
			to = parent.TransformPoint (to);

			// Get other two edges for brick direction
			HighLevelEdge e1 = null, e2 = null;
			foreach (HighLevelEdge e in edge.getFrom().getEdges()) {
				if (e.Equals (edge))
					continue;
				else if (e1 == null) {
					e1 = e;
				} else if (e2 == null) {
					e2 = e;
				} else {
					Debug.Log ("Warning more then 3 edges, likely invalid render");
				}
			}

			if (e2 == null) {
				Debug.Log ("Warning, corner with only one edge found, something went wrong!");
				return true;
			}

			// Calculate rotation for the two directions
			Vector3 e1Dir = e1.getDirection(cFrom).normalized;
			Vector3 e2Dir = e2.getDirection(cFrom).normalized;

			Quaternion rotationE1 = Quaternion.FromToRotation(Vector3.forward, e1Dir);
			Quaternion rotationE2 = Quaternion.FromToRotation (Vector3.forward, e2Dir);

			// Get direction and translate to world space
			Vector3 direction = edge.getDirection ();
			direction = parent.TransformDirection (direction);
			float edgeMagnitude = parent.TransformVector (direction).magnitude - cornerDimensions.y * 2;

			direction.Normalize ();

			// Set from and to actual center points of the corner
			from = from + Vector3.Scale (edge.getFrom ().getTranslateVector (), cornerDimensions/2) + Vector3.Scale(direction, cornerDimensions/2);
			to = to + Vector3.Scale (edge.getTo ().getTranslateVector (), cornerDimensions/2) - Vector3.Scale(direction, cornerDimensions/2);

			edgeMagnitude = (to-from).magnitude;

			// These are all world space
			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize();

			// Calculate start position
			Vector3 start = from + direction * dimensions.y / 2;

			// Create each cell
			int maxCount = (int) Mathf.Floor(edgeMagnitude / scale.y) - 1;
			int i = 0;
			bool takeDir1 = direction.y >= 0;

			float stepSize = dimensions.y;

			for (Vector3 p = start; i < maxCount; p += direction * stepSize) {
				Cell c;
				Color finalColor = Color.Lerp (color, Color.black, UnityEngine.Random.value * 0.3f);
				if (takeDir1) {
					c = new Cell (parent, p + (dimensions.z / 2 - cornerDimensions.z / 2) * e1Dir, scale, rotationE1, "CornerEdgeBrick");
				} else {
					c = new Cell (parent, p + (dimensions.z / 2 - cornerDimensions.z / 2) * e2Dir, scale, rotationE2, "CornerEdgeBrick");
				}
				takeDir1 = !takeDir1;
				c.setColor (finalColor);
				i++;
			}
			return true;
		}

		#endregion
	}
}

