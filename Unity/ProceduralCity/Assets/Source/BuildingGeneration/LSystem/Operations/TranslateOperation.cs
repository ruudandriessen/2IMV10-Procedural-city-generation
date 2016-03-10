using System;
using UnityEngine;

namespace ProceduralCity
{
	public class TranslateOperation : Operation
	{
		float tx, ty, tz;
		public TranslateOperation (GameObject g, float tx, float ty, float tz) : base(g)
		{
			this.tx = tx;
			this.ty = ty;
			this.tz = tz;
		}

		public override void applyOperation() {
			scope.transform.Translate (new Vector3 (tx, ty, tz));
		}
	}
}

