using System;
using UnityEngine;

namespace ProceduralCity
{
	public class ModelBlackBricks : ModelModule
	{
		public ModelBlackBricks (Transform parent)
		{
			this.parent = parent;
			Color colorWhite = Color.Lerp(Color.white, Color.black, 0.3f);
			Color colorBlack = Color.Lerp(Color.black, Color.white, 0.3f);
			this.setCornerModule (new CornerBlackBrick (parent, colorWhite));
			this.setVerticalEdgesModule (new EdgeHorizontalBricks (parent, colorBlack));
			this.setHorizontalEdgesModule (new EdgeBrick (parent, colorWhite));
			this.setVerticalRegionModule (new RegionBricks (parent, colorBlack));
			this.setHorizontalRegionModule (new RegionHorizontalBricks (parent, colorWhite));
		}

		public override bool apply (HighLevelMesh mesh)
		{
			// Apply all corners
			if (!this.applyCorners (mesh.getCorners ())) {
				Debug.Log ("Failed to apply corner module");
				return false;
			}

			// Apply all edges
			if (!this.applyEdges (mesh.getEdges ())) {
				Debug.Log ("Failed to apply edge module");
				return false;
			}

//			// Apply all regions
//			if (!this.applyRegions (mesh.getRegions ())) {
//				Debug.Log ("Failed to apply region module");
//				return false;
//			}
			return true;
		}
	}
}
