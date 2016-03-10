using System;
using UnityEngine;

namespace ProceduralCity
{
	abstract public class Operation
	{
		protected GameObject scope;
		public Operation(GameObject scope) {
			this.scope = scope;
		}

		abstract public void applyOperation();
	}
}

