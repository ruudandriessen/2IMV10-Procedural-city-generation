using System;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public abstract class Module
	{
		public enum ModuleInput {corner, edge, region, model, cell, allEdges, allCorners, allRegions};
		protected List<Module> children;
		protected ModuleInput inputType;

		public Module ()
		{
		}

		public ModuleInput getInputType() {
			return inputType;
		}
	}
}

