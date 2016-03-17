using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class HighLevelEdge
	{
		List<Edge> edges;
		List<Region> regions;
		Corner from, to;

		public HighLevelEdge ()
		{
			edges = new List<Edge> ();
			regions = new List<Region> ();
		}

		public void setFrom(Corner from) {
			this.from = from;
		}

		public Vector3 getDirection() {
			return to.getVertex ().getPoint () - from.getVertex ().getPoint ();
		}

		public void setTo(Corner to) {
			this.to = to;
		}

		public void addEdge(Edge e) {
			this.edges.Add (e);
		}

		public void addRegion(Region r) {
			this.regions.Add(r);
		}

		public void addEdgeRange(List<Edge> e) {
			this.edges.AddRange (e);
		}

		public List<Edge> getEdges() {
			return edges;
		}

		public List<Region> getRegions() {
			return regions;
		}

		public Corner getFrom() {
			return from;
		}

		public Corner getTo() {
			return to;
		}

		public void draw() {
			foreach (Edge e in edges) {
				e.draw ();
			}
		}
	}
}

