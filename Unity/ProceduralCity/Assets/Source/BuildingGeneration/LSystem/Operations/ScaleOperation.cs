using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class ScaleOperation : Operation
	{
		float sx, sy, sz;
		public ScaleOperation (Scope g, float sx, float sy, float sz) : base(g)
		{
			this.sx = sx;
			this.sy = sy;
			this.sz = sz;
		}

		public override void applyOperation(Symbol s, ref List<Symbol> symbols) {
			if (sx != 1 && sy != 1 && sz != 1) {
				setScale (sx, sy, sz);
				return;
			}
			int numberOfPoints = s.getPoints ().Count;
			for (int i = 0; i < numberOfPoints; i++) {
				Vector3 p = s.getPoint (i);
				s.addPoint(new Vector3(p.x, sy, p.z));
			}
		}
	}
}

