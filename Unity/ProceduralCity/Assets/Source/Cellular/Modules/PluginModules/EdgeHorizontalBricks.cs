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
			this.setCellDimensions(new Vector3(0.2f, 0.1f, 0.1f));
			this.setCellPadding(new Vector3(0.005f, 0.005f, 0.005f));
			this.color = c;
		}

		#region implemented abstract members of EdgeModule

		public override bool apply (HighLevelEdge edge)
		{
			Vector3 cornerDimensions = new Vector3 (0.1f, 0.1f, 0.1f);

			// Get from and translate to world space
			Vector3 from = edge.getFrom ().getVertex().getPoint();
			Vector3 to = edge.getTo ().getVertex().getPoint();
			from = parent.TransformPoint (from);
			to = parent.TransformPoint (to);

			// Get direction and translate to world space
			Vector3 direction = edge.getDirection ();
			float edgeMagnitude = parent.TransformVector (direction).magnitude - cornerDimensions.magnitude;

			// Set from and to actual center points of the corner
			from = from + Vector3.Scale (edge.getFrom ().getTranslateVector (), cornerDimensions / 2) + Vector3.Scale(direction, cornerDimensions /2);
			to = to + Vector3.Scale (edge.getTo ().getTranslateVector (), cornerDimensions / 2) - Vector3.Scale(direction, cornerDimensions / 2);


			// These are all world space
			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize();

			// Calculate rotation
//			Quaternion rotation = Quaternion.FromToRotation (Vector3.Scale(Vector3.up, direction), Vector3.right);

			// Calculate start position
			Vector3 start = from + direction * dimensions.x / 2;

			// Create each cell
			int maxCount = (int) Mathf.Floor(edgeMagnitude / scale.x);
			int i = 0;

			float stepSize = dimensions.x;

			for (Vector3 p = start; i < maxCount; p += direction * stepSize) {
				Cell c = new Cell (parent, p, scale, Quaternion.identity, "Brick");
				c.setColor (color);
				i++;
			}
			return true;
		}

		#endregion
	}
}

