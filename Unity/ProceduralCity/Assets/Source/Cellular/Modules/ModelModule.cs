using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public abstract class ModelModule : Module
	{
		private EdgeModule verticalEdges, horizontalEdges;
		private CornerModule cornerModule;
		private RegionModule horizontalRegion, verticalRegion;
		public ModelModule () : base()
		{
			this.inputType = ModuleInput.model;
		}

		public void setTransform(Transform parent) {
			this.parent = parent;
		}

		public abstract bool apply (HighLevelMesh mesh);

		// -- Corner handling --
		public void setCornerModule(CornerModule m) {
			this.cornerModule = m;
		}

		public CornerModule getCornerModule () {
			return cornerModule;
		}

		public List<MeshFilter> applyCorners(List<Corner> corners) {
			List<MeshFilter> meshes = new List<MeshFilter> ();
			if (cornerModule == null) {
				return meshes;
			}
			foreach (Corner c in corners) {
				// Apply to each corner
				meshes.AddRange(this.cornerModule.apply (c));
			}
			return meshes;
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

		public List<MeshFilter> applyEdges(List<HighLevelEdge> edges) {
			List<MeshFilter> meshes = new List<MeshFilter> ();
			if (verticalEdges == null && horizontalEdges == null) {
				return meshes;
			}
			foreach (HighLevelEdge e in edges) {
				if (e.getDirection ().y == 0 && horizontalEdges != null) {
					// Apply horizontal
					meshes.AddRange(this.horizontalEdges.apply (e));
				} else if (verticalEdges != null) {
					// Apply vertical
					meshes.AddRange(this.verticalEdges.apply (e));
				}
			}
			return meshes;
		}

		// -- Region handling --
		public void setRegionModule(RegionModule m) {
			this.verticalRegion = m;
			this.horizontalRegion = m;
		}

		public void setHorizontalRegionModule(RegionModule m) {
			this.horizontalRegion = m;
		}

		public RegionModule getHorizontalRegionModule () {
			return horizontalRegion;
		}

		public void setVerticalRegionModule(RegionModule m) {
			this.verticalRegion = m;
		}

		public RegionModule getVerticalRegionModule () {
			return verticalRegion;
		}

		public List<MeshFilter> applyRegions(List<Region> regions) {
			List<MeshFilter> meshes = new List<MeshFilter> ();
			foreach (Region r in regions) {
				if (r.isHorizontal ()) {
//					if (horizontalRegion == null || !this.horizontalRegion.apply (r)) {
//						// If it went wrong, return false
//						return false;
//					}
				} else {
					if (verticalRegion == null) {
						// If it went wrong, return false
						return meshes;;
					}
					meshes.AddRange(this.verticalRegion.apply (r));
				}
			}
			return meshes;
		}
	}
}

