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
		private List<Face> faces;

		public Vertex (Vector3 p) {
			this.p = p;
			edges = new List<Edge> ();
			faces = new List<Face> ();
		}

		public VertexLabel getLabel() {
			return lbl;
		}

		public List<Edge> getEdges() {
			return edges;
		}

		public void calculateLabel() {
			int numConvex = 0, numConcave = 0, numFlat = 0;
			for (int i = 0; i < edges.Count; i++) {
				Edge.EdgeLabel eLabel = edges [i].getLabel();
				switch (eLabel) {
					case Edge.EdgeLabel.concave:
						numConcave++;
						break;
					case Edge.EdgeLabel.convex:
						numConvex++;
						break;
					case Edge.EdgeLabel.flat:
						numFlat++;
						break;
				}
			}

			if (numFlat == edges.Count) {
				lbl = VertexLabel.inRegionFlat;
			} else if (edges.Count - numFlat == numConcave) {
				lbl = VertexLabel.cornerConcave;
			} else if (edges.Count - numFlat == numConvex) {
				lbl = VertexLabel.cornerConvex;
			} else if (numConvex > 0 && numConcave > 0) {
				lbl = VertexLabel.cornerSaddle;
			} else if (numConcave == 2) {
				lbl = VertexLabel.onEdgeConcave;
			} else if (numConvex == 2) {
				lbl = VertexLabel.onEdgeConvex;
			}
		}

		public void clearEdges() {
			edges.Clear ();
		}

		public void addEdge(Edge e){ 
			if (!edges.Contains(e))
				edges.Add (e);
		}

		public void addEdges(List<Edge> edges) {
			foreach (Edge e in edges) {
				this.addEdge (e);
			}
		}

		public void addFace(Face f) {
			faces.Add(f);
		}

		public List<Face> getFaces() {
			return faces;
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

