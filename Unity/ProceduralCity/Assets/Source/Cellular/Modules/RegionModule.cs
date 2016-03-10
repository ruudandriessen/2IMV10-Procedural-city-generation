using System;

namespace ProceduralCity
{
	public abstract class RegionModule : Module
	{
		public RegionModule ()
		{
			this.inputType = ModuleInput.region;
		}

		abstract public void apply(Region r);
	}
}

