using System;
using UnityEngine;

namespace ProceduralCity
{
	abstract public class Operation
	{
		protected Scope scope;
		public Operation(Scope scope) {
			this.scope = scope;
		}

		abstract public void applyOperation();

		protected GameObject getGameObject() {
			return this.scope.getGameObject();
		}

		protected float getSx() {
			return this.scope.getSx ();
		}

		protected float getSy() {
			return this.scope.getSy ();
		}

		protected float getSz() {
			return this.scope.getSz ();
		}

		protected void setScale(float x, float y, float z) {
			this.scope.setScope (x, y, z);
		}
	}
}

