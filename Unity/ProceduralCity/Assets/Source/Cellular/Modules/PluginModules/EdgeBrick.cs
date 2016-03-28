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
			this.setCellDimensions(new Vector3(1.0f, 1.0f, 2.0f));
			this.setCellPadding(new Vector3(0.005f, 0.005f, 0.005f));
			this.color = c;
		}

		#region implemented abstract members of EdgeModule

		public override bool apply (HighLevelEdge edge)
		{
			Vector3 cornerDimensions = new Vector3 (1.0f, 1.0f, 1.0f);

			// Get from and translate to world space
			Vector3 from = edge.getFrom ().getVertex().getPoint();
			Vector3 to = edge.getTo ().getVertex().getPoint();
			from = parent.TransformPoint (from);
			to = parent.TransformPoint (to);

			// Get direction and translate to world space
			Vector3 direction = edge.getDirection ();
			direction = parent.TransformDirection (direction);
			direction.Normalize ();

			// Set from and to actual center points of the corner
			from = from + Vector3.Scale (edge.getFrom ().getTranslateVector (), cornerDimensions / 2) + Vector3.Scale(direction, cornerDimensions /2);
			to = to + Vector3.Scale (edge.getTo ().getTranslateVector (), cornerDimensions / 2) - Vector3.Scale(direction, cornerDimensions / 2);

			float edgeMagnitude = (to-from).magnitude;

			// These are all world space
			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize();

			FillCellModule.fillCell (from, to, scale, dimensions, parent, color);

			return true;
		}

		#endregion
	}
}

