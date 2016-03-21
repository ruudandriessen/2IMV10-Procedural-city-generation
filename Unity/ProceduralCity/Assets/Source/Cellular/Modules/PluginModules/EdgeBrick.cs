using System;
using UnityEngine;

namespace ProceduralCity
{
	public class EdgeBrick : EdgeModule
	{
		Color color;
		public EdgeBrick (Transform parent, Color c)
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
			direction = parent.TransformDirection (direction);

			// Set from and to actual center points of the corner
			from = from + Vector3.Scale (edge.getFrom ().getTranslateVector (), cornerDimensions / 2) + Vector3.Scale(direction, cornerDimensions /2);
			to = to + Vector3.Scale (edge.getTo ().getTranslateVector (), cornerDimensions / 2) - Vector3.Scale(direction, cornerDimensions / 2);

			float edgeMagnitude = (to-from).magnitude;

			// These are all world space
			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize();

			// Calculate rotation
			Quaternion rotation = Quaternion.FromToRotation (direction, Vector3.right);

			// Calculate start position
			Vector3 start = from + direction * dimensions.x / 2;

			// Create each cell
			int maxCount = (int) Mathf.Floor(edgeMagnitude / scale.x) + 1;
			int i = 0;

			float stepSize = dimensions.x;

			for (Vector3 p = start; i < maxCount; p += direction * stepSize) {
				if (i == maxCount - 1) {
					Vector3 prevP = p - direction * stepSize;
					Vector3 prevPStart = prevP + direction * stepSize / 2;
					Vector3 overshoot = to - prevPStart;
					if (overshoot.magnitude > 0.001f) {
						Debug.Log(overshoot.ToString("F4"));
						scale.x = overshoot.x / 2;
						Vector3 targetPoint = prevPStart + 0.5f * (to - prevPStart);
						Cell c = new Cell (parent, targetPoint, scale, rotation, "Brick");
						c.setColor (color);
					}
				} else {
					Cell c = new Cell (parent, p, scale, rotation, "Brick");
					c.setColor (color);
				}
				i++;
			}
			return true;
		}

		#endregion
	}
}

