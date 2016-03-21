using System;
using UnityEngine;

namespace ProceduralCity
{
	public abstract class RegionModule : Module
	{
		protected Vector3 dimensions;
		protected Vector3 padding;

		public RegionModule () : base()
		{
			this.inputType = ModuleInput.region;
		}

		// Dimensions
		protected void setCellDimensions(Vector3 dim) {
			this.dimensions = dim;
		}

		public Vector3 getCellDimensions() {
			return dimensions;
		}

		public Vector3 getCellSize() {
			// Scale = <dimensions> - padding * 2 (both sides)
			return dimensions - padding * 2;
		}

		// Padding
		protected void setCellPadding(Vector3 p) {
			this.padding = p;
		}

		public Vector3 getCellPadding() {
			return padding;
		}

		abstract public bool apply(Region r);
	}
}

