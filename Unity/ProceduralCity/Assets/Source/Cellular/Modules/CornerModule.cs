using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public abstract class CornerModule : Module
	{
		protected Vector3 dimensions;
		protected Vector3 padding;

		public CornerModule () : base()
		{
			this.inputType = ModuleInput.corner;
		}

		// Dimensions
		protected void setDimensions(Vector3 dim) {
			this.dimensions = dim;
		}

		public Vector3 getDimensions() {
			return dimensions;
		}

		public Vector3 getCornerScale() {
			// Scale = <dimensions> - padding * 2 (both sides)
			return dimensions - padding * 2;
		}

		// Padding
		protected void setPadding(Vector3 p) {
			this.padding = p;
		}

		public Vector3 getPadding() {
			return padding;
		}

		abstract public List<MeshFilter> apply(Corner corner);
	}
}

