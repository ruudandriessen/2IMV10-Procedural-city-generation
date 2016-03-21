using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class CornerBlackBrick : CornerModule
	{
		public CornerBlackBrick (Transform parent)
		{
			this.parent = parent;
			this.setDimensions(new Vector3(0.1f, 0.1f, 0.1f));
			this.setPadding(new Vector3(0.01f, 0.01f, 0.01f));
		}

		public override bool apply (Corner corner)
		{
			Vector3 translateVector = corner.getTranslateVector ();
			Vector3 p = corner.getVertex ().getPoint ();
			Vector3 padding = getPadding ();

			p = parent.TransformPoint (p);

			Vector3 scale = this.getCornerScale ();

			p += Vector3.Scale(translateVector, dimensions / 2);

			Cell c = new Cell (parent, p, scale, Quaternion.identity, "BlackBrickCorner");
			c.setColor (Color.black);

			return true;
		}
	}
}

