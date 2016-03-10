using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class Edge { 
		public enum EdgeLabel {convex, flat, concave};

		EdgeLabel lbl;
		private Vector3 p1, p2;
		private List<Face> faces;

		public Edge (Vector3 p1, Vector3 p2) {
			this.p1 = p1;
			this.p2 = p2;
			this.faces = new List<Face> ();
		}

		public void setLabel(EdgeLabel lbl) {
			this.lbl = lbl;
			switch (lbl) {
			case EdgeLabel.concave: 
				Debug.DrawLine (p1, p2, Color.red, 200, true);
				break;
			case EdgeLabel.convex:
				Debug.DrawLine (p1, p2, Color.green, 200, true);
				break;
			case EdgeLabel.flat:
				Debug.DrawLine (p1, p2, Color.blue, 200, true);
				break;
			}
		}

		public Vector3 getFrom() {
			return p1;
		}	

		public Vector3 getTo() {
			return p2;
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
	}
}

