using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class SaveOperation : Operation
	{
		string shapeType;
		public SaveOperation (Scope g, string shapeType) : base(g)
		{
			this.shapeType = shapeType;
		}

		public override void applyOperation(Symbol s, ref List<Symbol> symbols) {
			Debug.Log ("Trying to save " + shapeType);
			Vector3[] points = new Vector3[s.getPoints ().Count];
			for (int i = 0; i < s.getPoints ().Count; i++) {
				points [i] = s.getPoint (i);
			}
			symbols.Add (new Symbol (shapeType, points));


		}
	}
}

