using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class Edge { 
		public enum EdgeLabel {convex, flat, concave};

		EdgeLabel lbl;
		private Vertex v1, v2;
		private List<Face> faces;
		bool processed;
		HighLevelEdge parent;

		public Edge (Vertex v1, Vertex v2) {
			this.v1 = v1;
			this.v2 = v2;
			this.faces = new List<Face> ();
		}
			
		public void setProcessed(bool state) {
			processed = state;
		}

		public bool isProcessed() {
			return processed;
		}

		public void setLabel(EdgeLabel lbl) {
			this.lbl = lbl;
//			Color c = Color.grey;
//			switch (lbl) {
//			case EdgeLabel.concave:
//				c = Color.red;
//				break;
//			case EdgeLabel.convex:
//				c = Color.blue;
//				break;
//			case EdgeLabel.flat:
//				c = Color.cyan;
//				break;
//			}
//			Debug.DrawLine(v1.getPoint(), v2.getPoint(), c, 200);
		}

		public Vertex getFrom() {
			return v1;
		}	

		public Vertex getTo() {
			return v2;
		}

		public void addFace(Face f) {
			this.faces.Add (f);
		}

		public List<Face> getFaces() {
			return faces;
		}

		public EdgeLabel getLabel() {
			return lbl;
		}

		public override bool Equals(System.Object obj)
		{
			if (obj == null)
				return false;
			Edge e = obj as Edge;
			if ((System.Object)e == null)
				return false;
			return (this == e);
		}

		public static bool operator ==(Edge e1, Edge e2) {
			return (e1.getTo() == e2.getFrom() && e1.getFrom() == e2.getTo()) || 
				(e1.getTo() == e2.getTo() && e1.getFrom() == e2.getFrom());
		}

		public static bool operator !=(Edge e1, Edge e2)
		{
			return !(e1 == e2);
		}

		public void setParent(HighLevelEdge e) {
			this.parent = e;
		}

		public void draw() {
			Debug.DrawLine (v1.getPoint (), v2.getPoint(), Color.red, 200);
		}
	}
}

