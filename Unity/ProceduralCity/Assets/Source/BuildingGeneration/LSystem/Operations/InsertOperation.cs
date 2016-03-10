using System;
using UnityEngine;

namespace ProceduralCity
{
	public class InsertOperation : Operation
	{
		PrimitiveType s;
		public InsertOperation (GameObject g, PrimitiveType shape) : base(g)
		{
			s = shape;
		}

		public override void applyOperation() {
			Debug.Log ("Shape gemaakt");
			GameObject shape = GameObject.CreatePrimitive (s);
			shape.transform.parent = scope.transform;

		}
	}
}

