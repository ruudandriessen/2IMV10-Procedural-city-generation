using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		public override void applyOperation(Symbol s, ref List<Symbol> symbols) {
			scope.setTranslation (tx, ty, tz);
		}
	}
}

