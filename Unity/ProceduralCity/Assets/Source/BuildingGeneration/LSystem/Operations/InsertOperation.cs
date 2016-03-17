using System;
using UnityEngine;

namespace ProceduralCity
{
	public class InsertOperation : Operation
	{
		PrimitiveType s;
		public InsertOperation (Scope g, PrimitiveType shape) : base(g)
		{
			s = shape;
		}

		public override void applyOperation() {
			Debug.Log ("Shape gemaakt");
			GameObject shape = GameObject.CreatePrimitive (s);
			shape.transform.parent = scope.getGameObject ().transform;
			shape.transform.localScale = new Vector3 (scope.getSx (), scope.getSy (), scope.getSz ());
			if (s == PrimitiveType.Cylinder) {
				shape.transform.Translate (new Vector3 (scope.getSx() / 2, scope.getSy(), scope.getSz() / 2));
			} else {
				shape.transform.Translate (new Vector3 (scope.getSx() / 2, scope.getSy() / 2, scope.getSz() / 2));
			}
			shape.transform.Translate (scope.getTranslationWithScalar ());
		}
	}
}

