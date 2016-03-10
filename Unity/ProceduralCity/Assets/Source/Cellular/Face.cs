using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class Face {
		private Vector3 center;
		private Vector3 normal;
		List<Edge> edges;
		List<Vertex> vertices;

		public Face(Vector3 c, Vector3 n) {
			this.center = c;
			this.normal = n;
			//Debug.DrawRay(c, n, Color.cyan, 200);
			this.edges = new List<Edge> ();
			this.vertices = new List<Vertex> ();
		}

		public void addEdge (Edge e) {
			edges.Add (e);
		}	

		public void addVertex (Vertex v) {
			vertices.Add (v);
		}

		public Vector3 getNormal() {
			return normal;
		}

		public Vector3 getCenter() {
			return center;
		}
	}

}

