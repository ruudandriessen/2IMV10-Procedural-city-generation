using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class CornerBlackBrick : CornerModule
	{
		Color color;
		public CornerBlackBrick (Transform parent, Color c)
		{
			this.parent = parent;
			this.setDimensions(new Vector3(0.1f, 0.1f, 0.1f));
			this.setPadding(new Vector3(0.001f, 0.001f, 0.001f));
			this.color = c;
		}

		public override bool apply (Corner corner)
		{
			Vector3 translateVector = corner.getTranslateVector ();
			Vector3 p = corner.getVertex ().getPoint ();

			p = parent.TransformPoint (p);

			Vector3 scale = this.getCornerScale ();

			p += Vector3.Scale(translateVector, dimensions / 2);

			Cell c = new Cell (parent, p, scale, Quaternion.identity, "BlackBrickCorner");
			Color finalColor = Color.Lerp (color, Color.black, UnityEngine.Random.value * 0.3f);
			c.setColor (finalColor);

			return true;
		}
	}
}

