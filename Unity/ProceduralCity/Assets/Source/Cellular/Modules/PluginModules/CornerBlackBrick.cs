﻿using System;
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
			this.setDimensions(new Vector3(1.0f, 1.0f, 1.0f));
			this.setPadding(new Vector3(0.05f, 0.05f, 0.05f));
			this.color = c;
		}

		public override List<MeshFilter> apply (Corner corner)
		{
			List<MeshFilter> meshes = new List<MeshFilter>();
			Vector3 translateVector = corner.getTranslateVector ();
			Vector3 p = corner.getVertex ().getPoint ();

			p = parent.TransformPoint (p);

			Vector3 scale = this.getCornerScale ();

			p += Vector3.Scale(translateVector, dimensions / 2);

			Cell c = new Cell (parent, p, scale, Quaternion.identity, "BrickCorner");
			Color finalColor = Color.Lerp (color, Color.black, UnityEngine.Random.value * 0.3f);
			c.setColor (finalColor);
			meshes.Add (c.getCell ().GetComponent<MeshFilter> ());
			return meshes;
		}
	}
}

