using System;
using UnityEngine;

namespace ProceduralCity
{
	public class TranslateOperation : Operation
	{
		float tx, ty, tz;
		public TranslateOperation (Scope g, float tx, float ty, float tz) : base(g)
		{
			this.tx = tx;
			this.ty = ty;
			this.tz = tz;
		}

		public override void applyOperation() {
			scope.setTranslation (tx, ty, tz);
		}
	}
}

