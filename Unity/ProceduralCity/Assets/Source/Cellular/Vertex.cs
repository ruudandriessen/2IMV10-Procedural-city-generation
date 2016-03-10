using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class Vertex {
		public enum VertexLabel {cornerConvex, cornerConcave, cornerSaddle, onEdgeConvex, onEdgeConcave, inRegionFlat};
		private Vector3 p;
		private VertexLabel lbl;
		private List<Edge> edges;

		public Vertex (Vector3 p) {
			this.p = p;
			edges = new List<Edge> ();
		}

		public void calculateLabel() {
			for (int i = 0; i < edges.Count; i++) {
				Edge.EdgeLabel eLabel = edges [i].getLabel();
			}
			this.lbl = lbl;
		}

		public void addEdge(Edge e){ 
			edges.Add (e);
		}

		public Vector3 getPoint() {
			return p;
		}

		public override bool Equals(System.Object obj)
		{
			// Return false if null or if it cannot be cast
			if (obj == null)
				return false;
			Vertex v = obj as Vertex;
			if ((System.Object)v == null)
				return false;
			// If the points are equal, so are the vertices
			return (this == v);
		}

		public static bool operator ==(Vertex v1, Vertex v2) {
			return v1.getPoint () == v2.getPoint ();
		}

		public static bool operator !=(Vertex v1, Vertex v2)
		{
			return !(v1 == v2);
		}
	}
}

