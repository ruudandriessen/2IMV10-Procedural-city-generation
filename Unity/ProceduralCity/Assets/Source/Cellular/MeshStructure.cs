using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public class MeshStructure
	{
		List<Vertex> vertices = new List<Vertex> ();
		List<Edge> edges = new List<Edge> ();
		List<Face> faces = new List<Face> ();

		public MeshStructure ()
		{
		}

		public void addEdge(Edge e) {
			if (!edges.Contains(e))
				this.edges.Add (e);
		}

		public void removeEdge(Edge e) {	
			this.edges.Remove (e);
		}

		public List<Edge> getEdges() {
			return edges;
		}

		public List<Vertex> getCornerVertices() {
			List<Vertex> cornerVertices = new List<Vertex> ();
			foreach (Vertex v in vertices) {
				Vertex.VertexLabel label = v.getLabel ();
				if (label == Vertex.VertexLabel.cornerConcave || label == Vertex.VertexLabel.cornerConvex || label == Vertex.VertexLabel.cornerSaddle) {
					cornerVertices.Add (v);
				}
			}
			return cornerVertices;
		}

		public List<Vertex> getVertices() {
			return vertices;
		}

		public List<Face> getFaces() {
			return faces;
		}

		public void removVertex(Vertex v) {
			this.vertices.Remove (v);
		}

		public void removeFace(Face f) {
			this.faces.Remove (f);
		}

		public void addVertex(Vertex v) {
			if (!vertices.Contains(v))
				this.vertices.Add (v);
		}

		public bool contains(Vertex v) {
			return vertices.Contains(v);
		}

		public void addFace(Face f) {
			if (!faces.Contains(f))
				this.faces.Add (f);
		}

		public int indexOfVertex(Vertex v) {
			return vertices.IndexOf(v);
		}

		public int indexOfEdge(Edge e) {
			return edges.IndexOf(e);
		}

		public Vertex getVertex(int i) {
			return vertices [i];
		}

		public Edge getEdge(int i) {
			return edges [i];
		}

		public void recalculateLabels() {
			foreach (Edge e in edges) {
				List<Face> edgeFaces = e.getFaces();
				if (edgeFaces.Count < 2) {
					continue;
				}

				Vector3 n1 = edgeFaces[0].getNormal();
				Vector3 c1 = edgeFaces [0].getCenter ();
				Vector3 c2 = edgeFaces [1].getCenter ();
				Vector3 w = c2 - c1;

				float angle = Vector3.Angle (w, n1);
				if (angle > 89.5f && angle <= 90.5f) {
					e.setLabel (Edge.EdgeLabel.flat);
				} else if (angle < 90) {
					e.setLabel (Edge.EdgeLabel.concave);
				} else {
					e.setLabel (Edge.EdgeLabel.convex);
				}
			}

			foreach (Vertex v in vertices) {
				v.calculateLabel ();
			}
		}
	}
}

