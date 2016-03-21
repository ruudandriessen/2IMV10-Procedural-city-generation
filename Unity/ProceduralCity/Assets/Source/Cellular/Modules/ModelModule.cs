using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public abstract class ModelModule : Module
	{
		private EdgeModule verticalEdges, horizontalEdges;
		private CornerModule cornerModule;

		public ModelModule () : base()
		{
			this.inputType = ModuleInput.model;
		}

		public abstract bool apply (HighLevelMesh mesh);

		// -- Corner handling --
		public void setCornerModule(CornerModule m) {
			this.cornerModule = m;
		}

		public CornerModule getCornerModule () {
			return cornerModule;
		}

		public bool applyCorners(List<Corner> corners) {
			if (cornerModule == null) {
				return false;
			}
			foreach (Corner c in corners) {
				// Apply to each corner
				if (!this.cornerModule.apply (c)) {
					// If it went wrong, return false
					return false;
				}
			}
			return true;
		}

		// -- Edge handling --
		public void setVerticalEdgesModule(EdgeModule m) {
			this.verticalEdges = m;
		}

		public void setHorizontalEdgesModule(EdgeModule m) {
			this.horizontalEdges = m;
		}

		public void setEdgeModule(EdgeModule m) {
			this.verticalEdges = m;
			this.horizontalEdges = m;
		}

		public EdgeModule getVerticalEdgesModule() {
			return verticalEdges;
		}

		public EdgeModule getHorizontalEdgesModule() {
			return horizontalEdges;
		}

		public bool applyEdges(List<HighLevelEdge> edges) {
			if (verticalEdges == null && horizontalEdges == null) {
				return false;
			}
			foreach (HighLevelEdge e in edges) {
				if (e.getDirection ().y == 0 && horizontalEdges != null) {
					// Apply horizontal
					if (!this.horizontalEdges.apply (e)) {
						// If it went wrong, return false
						return false;
					}
				} else if (verticalEdges != null) {
					// Apply vertical
					if (!this.verticalEdges.apply (e)) {
						// If it went wrong, return false
						return false;
					}
				}
			}
			return true;
		}
	}
}

