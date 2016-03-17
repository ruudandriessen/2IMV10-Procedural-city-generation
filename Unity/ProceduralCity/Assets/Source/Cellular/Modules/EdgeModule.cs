using System;
using UnityEngine;

namespace ProceduralCity
{
	public abstract class EdgeModule : Module
	{
		public EdgeModule () : base()
		{
			this.inputType = ModuleInput.edge;
		}

		abstract public void apply(HighLevelEdge e);
	}
}

