using System;

namespace ProceduralCity
{
	public abstract class ModelModule : Module
	{
		public ModelModule () : base()
		{
			this.inputType = ModuleInput.model;
		}

		public abstract void apply (HighLevelMesh mesh);
	}
}

