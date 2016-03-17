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
		bool processed;
		Region parent;

		public Face(Vector3 c, Vector3 n) {
			this.center = c;
			this.normal = n;
			this.edges = new List<Edge> ();
			this.vertices = new List<Vertex> ();
		}
			
		public void setProcessed(bool state) {
			processed = state;
		}

		public bool isProcessed() {
			return processed;
		}

		public bool hasEdge(Edge e) {
			return this.edges.Contains (e);
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

		public List<Vertex> getVertices() {
			return vertices;
		}

		public bool sharesVertex(Face f) {
			foreach (Vertex v1 in vertices) {
				foreach (Vertex v2 in f.getVertices()) {
					if (v2.Equals (v1))
						return true;
				}
			}
			return false;
		}

		public void setParent(Region r) {
			this.parent = r;
		}

		public Region getParent() {
			return parent;
		}

		public void draw() {
			foreach (Edge e in edges) {
				e.draw ();
			}
		}
	}

}

