using System;
using UnityEngine;

namespace ProceduralCity
{
	public class RegionHorizontalBricks : RegionModule
	{
		Color color;
		public RegionHorizontalBricks (Transform parent, Color c)
		{
			this.parent = parent;
			this.setCellDimensions(new Vector3(4.0f, 1.0f, 4.0f));
			this.setCellPadding(new Vector3(0.005f, 0.005f, 0.005f));
			this.color = c;
		}

		#region implemented abstract members of RegionModule

		public override bool apply (Region r)
		{
			Vector3 cornerDimensions = new Vector3 (2.0f, 2.0f, 2.0f);

			if (r.getEdges ().Count == 0) {
				Debug.Log ("Warning - no edges found near region, error");
				return true;
			}
			HighLevelEdge e1 = r.getEdges ()[0];
			HighLevelEdge e2 = null;
			foreach (HighLevelEdge edge in r.getEdges ()) {
				if (edge.Equals (e1))
					continue;
				if( e1.getFrom().Equals(edge.getFrom()) ) {
					// Same start vertex
					e2 = edge;
					break;
				}
			}
			if (e2 == null) {
				Debug.Log ("Error - no two edges from same start vertex in region");
				return true;
			}
			Vector3 dir1, dir2;
			dir1 = e1.getDirection ();
			dir2 = e2.getDirection ();
			Corner corner = e1.getFrom ();
			Vector3 p = corner.getVertex().getPoint();
			dir1 = parent.TransformVector(dir1);
			dir2 = parent.TransformVector(dir2);
			p = parent.TransformPoint (p);

			// Calculate rotation
			Quaternion rotation = Quaternion.FromToRotation (Vector3.right, dir1);

			float dir1Magnitude = dir1.magnitude;
			float dir2Magnitude = dir2.magnitude;

			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize ();

			dir1 = parent.InverseTransformVector (dir1);
			dir2 = parent.InverseTransformVector (dir2);
			dir1.Normalize ();
			dir2.Normalize ();

			Vector3 start = p + Vector3.Scale(corner.getTranslateVector(), cornerDimensions / 2);
			start += cornerDimensions.x / 2 * dir1 + dimensions.x / 2 * dir1;
			start += cornerDimensions.y / 2 * dir2 + dimensions.y / 2 * dir2;

			int maxDir1 = (int) (dir1Magnitude / dimensions.x);
			int maxDir2 = (int) (dir2Magnitude / dimensions.z) - 1;

			MeshFilter[] meshFilters = new MeshFilter[maxDir1 * maxDir2];
			for (int i = 0; i < maxDir1; i++) {
				for (int j = 0; j < maxDir2; j++) {
					Color finalColor = Color.Lerp (color, Color.black, UnityEngine.Random.value * 0.3f);
					Vector3 location = start + i * dimensions.x * dir1;
					location += j * dimensions.z * dir2;
					Cell c = new Cell (parent, location, scale, rotation, "Region brick");
					c.setColor (finalColor);
					meshFilters [i * maxDir2 + j] = c.getCell ().GetComponent<MeshFilter>();
				}
			}	

			// Optimize by combining mesh
			CombineInstance[] combine = new CombineInstance[meshFilters.Length];
			GameObject region = new GameObject ();
			MeshFilter filter = region.AddComponent<MeshFilter> ();
			region.name = "HorzRegion";
			int index = 0;
			while (index < meshFilters.Length) {
				combine[index].mesh = meshFilters[index].sharedMesh;
				combine[index].transform = meshFilters[index].transform.localToWorldMatrix;
				meshFilters [index].gameObject.SetActive (false);
				index++;
			}
			filter.mesh = new Mesh();
			filter.mesh.CombineMeshes(combine);
			region.transform.gameObject.SetActive (true);

			return true;
		}

		#endregion
	}
}

