using System;
using UnityEngine;

namespace ProceduralCity
{
	public abstract class EdgeModule : Module
	{
		Vector3 dimensions;
		Vector3 padding;

		public EdgeModule () : base()
		{
			this.inputType = ModuleInput.edge;
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

		abstract public bool apply(HighLevelEdge e);
	}
}

