using System;

namespace ProceduralCity
{
	public abstract class RegionModule : Module
	{
		public RegionModule () : base()
		{
			this.inputType = ModuleInput.region;
		}

		abstract public bool apply(Region r);
	}
}

