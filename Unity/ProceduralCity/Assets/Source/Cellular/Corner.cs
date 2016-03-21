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

		public Vector3 getNormal() {
			Vector3 normal = Vector3.zero;
			foreach (HighLevelEdge e in getEdges()) {
				foreach (Region r in e.getRegions()) {
					normal += r.getNormal ();
				}
			}
			return normal.normalized;
		}

		public Vector3 getTranslateVector() {
			Vector3 normal = this.getNormal ();
			// Get translate vector according to normal
			float transX = normal.x > 0 ? -1 : normal.x < 0 ? 1 : 0;
			float transY = normal.y > 0 ? -1 : normal.y < 0 ? 1 : 0;
			float transZ = normal.z > 0 ? -1 : normal.z < 0 ? 1 : 0;
			Vector3 translateVector = new Vector3 (transX, transY, transZ);
			return translateVector;
		}

		public Vertex getVertex() {
			return vertex;
		}
	}
}

