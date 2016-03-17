using System;
using UnityEngine;

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

		public override void applyOperation() {
			setScale (sx, sy, sz);
		}
	}
}

