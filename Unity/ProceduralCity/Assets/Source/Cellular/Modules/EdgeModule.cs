using System;
using UnityEngine;

namespace ProceduralCity
{
	public abstract class EdgeModule : Module
	{
		public EdgeModule ()
		{
			this.inputType = ModuleInput.edge;
		}

		abstract public void apply(Edge e);
	}
}

