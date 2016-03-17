using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ProceduralCity
{
	public class Region
	{
		private List<Face> faces;
		private List<HighLevelEdge> edges;
		private Vector3 normal;

		public Region ()
		{
			edges = new List<HighLevelEdge> ();
			faces = new List<Face> ();
		}

		public Vector3 getNormal() {
			return normal;
		}

		public void addEdge(HighLevelEdge e) {
			this.edges.Add (e);
		}

		public bool attemptMerge(Region r) {
			if (normal != r.getNormal ()) {
				// We definitly can't merge a region with another normal
				return false;
			}

			foreach (Face f1 in r.getFaces()) {
				foreach (Face f2 in faces) {
					if (f1.sharesVertex(f2)) {
						mergeWithRegion (r);
						return true;
					}
				}
			}
			return false;
		}

		private void mergeWithRegion(Region r) {
//			Debug.Log ("Merged, old: "+ faces.Count + "/" + edges.Count);
			this.faces.AddRange(r.getFaces ());
			this.edges.AddRange(r.getEdges ());
//			Debug.Log ("Merged, new: "+ faces.Count + "/" + edges.Count);
		}

		public List<HighLevelEdge> getEdges() {
			return edges;
		}

		public List<Face> getFaces() {
			return faces;
		}

		public bool addFace(Face f) {
			if (faces.Count == 0) {
				// We take this face as our base normal
				normal = f.getNormal ();

//				Debug.Log ("Region now have normal: " + normal);

				// And add the face
				this.faces.Add(f);
				return true;
			} else {
				// The normal does not match, this can't be in our region
				if (f.getNormal () == normal) {
					this.faces.Add (f);
					return true;
				}
			}
			return false;
		}

		public void draw() {
			foreach (HighLevelEdge e in edges) {
				e.draw ();
			}
		}
	}
}

