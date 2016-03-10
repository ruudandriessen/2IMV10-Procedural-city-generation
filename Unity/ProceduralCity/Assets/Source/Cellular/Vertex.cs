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

			Color activeColor = Color.yellow;
			if (numFlat == edges.Count) {
				// In region
				activeColor = Color.white;
				lbl = VertexLabel.inRegionFlat;
			} else if (edges.Count - numFlat == numConcave) {
				// Concave corner
				activeColor = Color.magenta;
				lbl = VertexLabel.cornerConcave;
			} else if (edges.Count - numFlat == numConvex) {
				// Convex corner
				activeColor = Color.cyan;
				lbl = VertexLabel.cornerConvex;
			} else if (numConvex > 0 && numConcave > 0) {
				// Corner saddle
				activeColor = Color.red;
				lbl = VertexLabel.cornerSaddle;
			} else if (numConcave == 2) {
				// Edge
				activeColor = Color.black;
				lbl = VertexLabel.onEdgeConcave;
			} else if (numConvex == 2) {
				activeColor = Color.gray;
				lbl = VertexLabel.onEdgeConvex;
			}
			float size = 0.2f;
			Debug.DrawRay (p, Vector3.up * size, activeColor, 200);
			Debug.DrawRay (p, Vector3.down * size, activeColor, 200);
			Debug.DrawRay (p, Vector3.left * size, activeColor, 200);
			Debug.DrawRay (p, Vector3.right * size, activeColor, 200);
			Debug.DrawRay (p, Vector3.forward * size, activeColor, 200);
			Debug.DrawRay (p, Vector3.back * size, activeColor, 200);
		}

		public void addEdge(Edge e){ 
			if (!edges.Contains(e))
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

