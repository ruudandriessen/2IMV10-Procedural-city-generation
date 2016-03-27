using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class ComplexCell
	{
		GameObject obj;

		public ComplexCell (List<Vector3> poly3D, float height, String name)
		{ 
			List<Vector2> polygon = new List<Vector2> ();
			foreach (Vector3 v3 in poly3D) {
				polygon.Add(new Vector2(v3.x, v3.z));
			}

			Triangulator tr = new Triangulator (polygon.ToArray());
			int[] indices = tr.Triangulate ();

			// Create the Vector3 vertices
			Vector3[] vertices = new Vector3[polygon.Count];
			for (int i=0; i<vertices.Length; i++) {
				vertices[i] = new Vector3(polygon[i].x, 0, polygon[i].y);
			}

			// Add rigidbody and disable movement by default
//			Rigidbody body = cell.AddComponent<Rigidbody> ();
//			body.useGravity = true;
//			body.isKinematic = true;

			// Create the mesh
			Mesh msh = new Mesh();
			msh.vertices = vertices;
			msh.triangles = indices;
			msh.RecalculateNormals();
			msh.RecalculateBounds();

			// Create game object
			obj.AddComponent(typeof(MeshRenderer));
			MeshFilter filter = obj.AddComponent(typeof(MeshFilter)) as MeshFilter;
			filter.mesh = msh;

			// Get renderer and apply material
			MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
			Material newMat = Resources.Load("Materials/Concrete_Asphalt_02", typeof(Material)) as Material;
			renderer.material = newMat;

			obj.name = name;
		}

		public void setColor(Color c) {
			obj.GetComponent<Renderer> ().material.color = c;
		}

		static public Vector3 div(Vector3 v1, Vector3 v2) {
			v1.x /= v2.x;
			v1.y /= v2.y;
			v1.z /= v2.z;
			return v1;
		}
	}
}

