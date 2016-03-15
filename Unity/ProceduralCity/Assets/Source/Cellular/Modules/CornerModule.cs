using System;
using UnityEngine;

namespace ProceduralCity
{
	public abstract class CornerModule : Module
	{
		public CornerModule () : base()
		{
			this.inputType = ModuleInput.corner;
		}

		abstract public void apply(Vertex corner);
	}
}

