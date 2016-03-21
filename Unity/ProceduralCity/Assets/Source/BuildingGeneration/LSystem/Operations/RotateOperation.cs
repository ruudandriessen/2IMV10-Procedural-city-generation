using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class RotateOperation : Operation
	{
		float rx, ry, rz;
		public RotateOperation (Scope g, float rx, float ry, float rz) : base(g)
		{
			this.rx = rx;
			this.ry = ry;
			this.rz = rz;
		}

		public override void applyOperation(Symbol s, ref List<Symbol> symbols) {
			scope.getGameObject().transform.Rotate (new Vector3 (rx, ry, rz));
		}
	}
}

