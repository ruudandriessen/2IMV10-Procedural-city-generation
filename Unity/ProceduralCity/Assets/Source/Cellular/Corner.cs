using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public class Corner
	{
		Vertex vertex;
		List<HighLevelEdge> edges;

		public Corner (Vertex v)
		{
			this.vertex = v;
			edges = new List<HighLevelEdge> ();
		}

		public void addEdge(HighLevelEdge e) {
			if (edges.Contains (e))
				return;
			this.edges.Add (e);
		}

		public List<HighLevelEdge> getEdges() {
			return edges;
		}

		public Vertex getVertex() {
			return vertex;
		}
	}
}

