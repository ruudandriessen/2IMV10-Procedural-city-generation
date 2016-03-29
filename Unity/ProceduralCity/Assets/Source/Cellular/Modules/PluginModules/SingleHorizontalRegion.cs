//using System;
//using UnityEngine;
//using System.Collections.Generic;
//
//namespace ProceduralCity
//{
//	public class SingleHorizontalRegion : RegionModule
//	{
//		Color color;
//		public SingleHorizontalRegion (Transform parent, Color c)
//		{
//			this.parent = parent;
//			this.setCellDimensions(new Vector3(4.0f, 1.0f, 4.0f));
//			this.setCellPadding(new Vector3(0.005f, 0.005f, 0.005f));
//			this.color = c;
//		}
//
//		public override bool apply (Region r)
//		{
//			float height = 0;
//			List<Vector2> points = new List<Vector2> ();
//			foreach (HighLevelEdge e in r.getEdges()) {
//				Vector3 p1 = e.getFrom ().getVertex ().getPoint ();
//				Vector3 p2 = e.getTo ().getVertex ().getPoint ();
//				Vector2 p1_2 = new Vector2(p1.x, p1.z);
//				Vector2 p2_2 = new Vector2(p2.x, p2.z);
//				height = p1.y;
//				if (!points.Contains(p1_2))
//					points.Add (p1_2);
//				if (!points.Contains(p2_2))
//					points.Add (p2_2);
//			}
//
//			// Use the triangulator to get indices for creating triangles
//			Triangulator tr = new Triangulator(points.ToArray());
//			int[] indices = tr.Triangulate();
//
//			// Create the Vector3 vertices
//			Vector3[] vertices = new Vector3[points.Count];
//			for (int i=0; i<vertices.Length; i++) {
//				vertices[i] = new Vector3(points[i].x, height, points[i].y);
//			}
//
//			// Create the mesh
//			Mesh msh = new Mesh();
//			msh.vertices = vertices;
//			msh.triangles = indices;
//			msh.RecalculateNormals();
//			msh.RecalculateBounds();
//
//			// Set up game object with mesh;
//			GameObject gameObject = new GameObject();
//			gameObject.AddComponent(typeof(MeshRenderer));
//			MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
//			filter.mesh = msh;
//
//			return true;
//		}
//	}
//}
