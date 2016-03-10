using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class HighLevelMesh
	{
		private List<Edge> edges;
		private List<Region> regions;
		private MeshStructure sourceMesh;

		public HighLevelMesh (MeshStructure structure)
		{
			sourceMesh = structure;
			edges = new List<Edge> ();
			regions = new List<Region> ();
		}

		public void construct() {
			foreach (Vertex v in sourceMesh.getCornerVertices()) {
				edges.AddRange(constructEdge (v));
			}
			foreach (Edge e in edges) {
				Debug.DrawLine (e.getFrom ().getPoint(), e.getTo ().getPoint(), Color.red, 200);
			}
		}

		public List<Edge> constructEdge(Vertex source) {
			Vertex.VertexLabel vertexLabel = source.getLabel ();
			if (vertexLabel == Vertex.VertexLabel.cornerConvex) {
				return constructEdge (source, true);
			} else {
				return constructEdge (source, false);
			}
		}

		public List<Edge> constructEdge(Vertex source, bool convex) {
			List<Edge> edges = new List<Edge>();

			foreach (Edge e in source.getEdges()) {
				Edge.EdgeLabel edgeLabel = e.getLabel ();
				Vertex target = e.getTo ();
				if (convex) {
					if (target.getLabel () == Vertex.VertexLabel.onEdgeConvex && edgeLabel == Edge.EdgeLabel.convex) {
						edges.AddRange(constructEdge (source, true));
					}
					if (target.getLabel () == Vertex.VertexLabel.cornerConvex && edgeLabel == Edge.EdgeLabel.convex) {
						edges.Add(new Edge (source, target));
					}
				} else {
					if (target.getLabel () == Vertex.VertexLabel.onEdgeConcave && edgeLabel == Edge.EdgeLabel.concave) {
						edges.AddRange (constructEdge(source, true));
					}
					if (target.getLabel () == Vertex.VertexLabel.cornerConcave && edgeLabel == Edge.EdgeLabel.concave) {
						edges.Add(new Edge (source, target));
					}
				}
			}

			return edges;
		}
	}
}

