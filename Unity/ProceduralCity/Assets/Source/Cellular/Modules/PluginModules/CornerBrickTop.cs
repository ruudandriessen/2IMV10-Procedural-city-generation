using System;
using UnityEngine;

namespace ProceduralCity
{
	public class CornerBrickTop : CornerModule
	{
		Color color;
		public CornerBrickTop (Transform parent, Color c)
		{
			this.parent = parent;
			this.setDimensions(new Vector3(0.01f, 0.1f, 0.2f));
			this.setPadding(new Vector3(0.005f, 0.005f, 0.005f));
			this.color = c;
		}

		public override bool apply (Corner corner)
		{
			Vector3 normal = corner.getNormal ();
			Vector3 p = corner.getVertex ().getPoint ();

			p = parent.TransformPoint (p);
			Vector3 scale = this.getCornerScale ();

			foreach (HighLevelEdge e in corner.getEdges()) {
				Vector3 direction = e.getDirection (corner).normalized;
//				Debug.DrawRay (p, direction * 0.2f, Color.cyan, 200);
				Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, direction);
				Vector3 location = p + direction * scale.z / 2 + e.getNormal () * scale.x / 2;
				Cell c = new Cell (parent, location, scale, rotation, "BrickCorner");
			}


//			p += Vector3.Scale(translateVector, dimensions / 2);

//			Cell c = new Cell (parent, p, scale, Quaternion.identity, "BrickCorner");
//			Color finalColor = Color.Lerp (color, Color.black, UnityEngine.Random.value * 0.3f);
//			c.setColor (finalColor);

			return true;
		}
	}
}

