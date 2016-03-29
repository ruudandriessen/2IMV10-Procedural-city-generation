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

		public Vector3 getDirection(Corner c) {
			if (c.Equals(from))
				return to.getVertex ().getPoint () - from.getVertex ().getPoint ();
			else
				return from.getVertex ().getPoint () - to.getVertex ().getPoint ();
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

		public Vector3 getNormal() {
			Vector3 normal = Vector3.zero;
			foreach (Region r in regions) {
				normal += r.getNormal();
			}
			normal /= regions.Count;
			return normal;
		}

		public Vector3 getTranslateVector() {
			// Get translate vector according to normal
			Vector3 normal = this.getNormal();
			// Get translate vector according to normal
			float transX = normal.x > 0 ? -1 : normal.x < 0 ? 1 : 0;
			float transY = normal.y > 0 ? -1 : normal.y < 0 ? 1 : 0;
			float transZ = normal.z > 0 ? -1 : normal.z < 0 ? 1 : 0;
			Vector3 translateVector = new Vector3 (transX, transY, transZ);
			return translateVector;
		}

		public void draw() {
			foreach (Edge e in edges) {
				e.draw ();
			}
		}
	}
}

