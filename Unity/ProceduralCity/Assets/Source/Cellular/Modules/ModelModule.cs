using System;

namespace ProceduralCity
{
	public abstract class ModelModule : Module
	{
		public ModelModule ()
		{
			this.inputType = ModuleInput.model;
		}

		public abstract void apply (HighLevelMesh mesh);
	}
}

