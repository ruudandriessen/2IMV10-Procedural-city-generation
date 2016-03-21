using System;

namespace ProceduralCity
{
	public class InsertModelOperation : Operation
	{
		string model;
		public InsertModelOperation (Scope g, string model) : base(g)
		{
			this.model = model;
		}

		public override void applyOperation(Symbol s, ref List<Symbol> symbols) {
			Debug.Log ("Shape gemaakt");
			GameObject shapeg = GameObject.CreatePrimitive (shape);
			shapeg.transform.parent = scope.getGameObject ().transform;
			shapeg.transform.localScale = new Vector3 (scope.getSx (), scope.getSy (), scope.getSz ());
			if (shape == PrimitiveType.Cylinder) {
				shapeg.transform.Translate (new Vector3 (scope.getSx() / 2, scope.getSy(), scope.getSz() / 2));
			} else {
				shapeg.transform.Translate (new Vector3 (scope.getSx() / 2, scope.getSy() / 2, scope.getSz() / 2));
			}
			shapeg.transform.Translate (scope.getTranslationWithScalar ());
		}
	}
}

