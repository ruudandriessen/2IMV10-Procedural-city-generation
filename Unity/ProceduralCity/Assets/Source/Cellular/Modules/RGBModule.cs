using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public class RGBModule : ModelModule
	{
		class RedCell : CornerModule {
			Transform parent;
			public RedCell(Transform parent) : base() {
				this.parent = parent;
			}

			float size = 0.1f; 

			public override void apply(Vertex v) {
				Vector3 p = v.getPoint ();
				p = parent.TransformPoint (p);

				Vector3 translationDirection = Vector3.zero;
				foreach (Edge e in v.getEdges()) {
					Vector3 direction = e.getTo ().getPoint() - e.getFrom ().getPoint();
					translationDirection += direction;
				}
				translationDirection /= v.getEdges ().Count;
				Vector3 trans = translationDirection.normalized * (size / 1.2f);

//				Debug.Log (translationDirection);

				Vector3 scale = new Vector3 (size, size, size);
				scale = div(scale, parent.lossyScale);
				p += trans;

				Cell c = new Cell (parent, p, scale, Quaternion.identity);
				c.setColor (Color.red);
			}
		}

		class GreenCell : EdgeModule {
			Transform parent;
			public GreenCell(Transform parent) : base() {
				this.parent = parent;
			}

			float cellTargetSize = 0.1f;
			float cellPadding = 0.02f;

			public override void apply(Edge e) {
				Vector3 p1 = e.getFrom ().getPoint (); 
				Vector3 p2 = e.getTo ().getPoint ();

				p1 = parent.TransformPoint (p1);
				p2 = parent.TransformPoint (p2);

				Vector3 direction = (p1 - p2);
				float edgeSize = direction.magnitude;
				edgeSize -= 0.1f * 3; // Size of corner
				direction.Normalize();

				int estCellCount = (int) Math.Floor(edgeSize / cellTargetSize);
				float actualCellSize = edgeSize / estCellCount;
				int actualCellCount = (int) (edgeSize / actualCellSize);
				
				Vector3 scale = new Vector3 (actualCellSize - cellPadding,
					actualCellSize - cellPadding, 
					actualCellSize - cellPadding);
				scale = div(scale, parent.lossyScale);

				Vector3 normal = Vector3.zero;
				foreach (Face f in e.getFaces()) {
					normal += f.getNormal ();
				}
				normal /= e.getFaces ().Count;

				for (int i = 0; i < actualCellCount; i++) {
					Vector3 position = (p1 - direction * i * actualCellSize) - direction * 0.1f * 1.5f;
					position += normal.normalized * (actualCellSize - cellPadding);
					Cell c = new Cell (parent, position, scale, Quaternion.identity);
					c.setColor (Color.green);
				}
			}
		}

		class BlueCell : RegionModule {
			public BlueCell() : base() {}

			float cellTargetSize = 0.1f;
			float cellPadding = 0.01f;

			public override void apply(Region r) {
				r.getPoints ();

//				float edgeSize = direction.magnitude;
//
//				int estCellCount = (int) Math.Floor(edgeSize / cellTargetSize);
//
//				float actualCellSize = edgeSize / estCellCount;
//				int actualCellCount = (int) (edgeSize / actualCellSize);
//
//				for (int i = 1; i < actualCellCount - 1; i++) {
//					Vector3 scale = new Vector3 (actualCellSize - cellPadding,
//						actualCellSize - cellPadding, 
//						actualCellSize - cellPadding);
//					Cell c = new Cell ((p1 - direction * i * actualCellSize), scale, Quaternion.identity);
//					c.setColor (Color.blue);
//				}
			}
		}

		Transform parent;
		public RGBModule (Transform parent) : base()
		{
			this.parent = parent;
		}

		public override void apply(HighLevelMesh mesh) { 
			RedCell cornerModule = new RedCell (parent);
			GreenCell edgeModule = new GreenCell (parent);
			foreach (Vertex v in mesh.getCorners()) {
				cornerModule.apply (v);
			}
			foreach (Edge e in mesh.getEdges()) {
				edgeModule.apply (e);
			}
		}

		static public Vector3 div(Vector3 v1, Vector3 v2) {
			v1.x /= v2.x;
			v1.y /= v2.y;
			v1.z /= v2.z;
			return v1;
		}
	}
}

