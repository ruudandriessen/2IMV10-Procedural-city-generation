using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public class RGBModule : ModelModule
	{
		/*
		 * Fills corners with red cells
		 */
		class RedCell : CornerModule {
			Transform parent;
			public RedCell(Transform parent) : base() {
				this.parent = parent;
			}

			float size = 0.1f; 
			float cellPadding = 0.01f;

			public override void apply(Corner corner) {
				Vector3 p = corner.getVertex ().getPoint();
				p = parent.TransformPoint (p);

				Vector3 translationDirection = Vector3.zero;
				foreach (HighLevelEdge e in corner.getEdges()) {
					foreach (Region r in e.getRegions()) {
						p -= (r.getNormal ().normalized) * (size / 4);
					}
				}

				Vector3 scale = new Vector3 (size - cellPadding, size - cellPadding, size - cellPadding);
				scale = div(scale, parent.lossyScale);

				Cell c = new Cell (parent, p, scale, Quaternion.identity, " Corner");
				c.setColor (Color.red);
			}
		}

		/*
		 * Fills edges with green cells
		 */
		class GreenCell : EdgeModule {
			Transform parent;
			public GreenCell(Transform parent) : base() {
				this.parent = parent;
			}

			float cellTargetSize = 0.1f;
			float cellPadding = 0.01f;

			public override void apply(HighLevelEdge e) {
				Corner c1 = e.getFrom (); 
				Corner c2 = e.getTo ();

				// Get the corner points
				Vector3 p1 = c1.getVertex ().getPoint ();
				Vector3 p2 = c2.getVertex ().getPoint ();

				// Transform to local space
				p1 = parent.TransformPoint (p1);
				p2 = parent.TransformPoint (p2);

				// Get direction of the edge
				Vector3 direction = (p1 - p2);
				float edgeSize = direction.magnitude;
				edgeSize -= 0.1f * 2; // Minus 2x size of a corner
				direction.Normalize();

				int estCellCount = (int) Math.Floor(edgeSize / cellTargetSize);
				float actualCellSize = edgeSize / estCellCount;
				int actualCellCount = (int) (edgeSize / actualCellSize);
				
				Vector3 scale = new Vector3 (actualCellSize - cellPadding,
					actualCellSize - cellPadding, 
					actualCellSize - cellPadding);
				scale = div(scale, parent.lossyScale);

				for (int i = 0; i < actualCellCount; i++) {
					Vector3 position = (p1 - direction * i * actualCellSize) - direction * cellTargetSize * 1.5f;
					foreach (Region r in e.getRegions()) {
						Vector3 normal = r.getNormal ().normalized;
						position -= normal * (actualCellSize/2 - cellPadding/4);
					}
					Cell c = new Cell (parent, position, scale, Quaternion.identity, "Edge");
					c.setColor (Color.green);
				}
			}
		}

		/*
		 * Fills regions with blue cells
		 */
		class BlueCell : RegionModule {
			Transform parent;
			public BlueCell(Transform parent) : base() {
				this.parent = parent;
			}

			float cellTargetWidth = 0.1f;
			float cellTargetHeight = 0.05f;
			float cellPadding = 0.005f;

			public override void apply(Region r) {
				HighLevelEdge hle1 = null, hle2 = null;

				// Take one of the edges as a basis
				hle1 = r.getEdges () [0];

				// Get it's direction
				Vector3 hle1Dir = hle1.getDirection ();
				foreach (HighLevelEdge e1 in r.getEdges()) {
					if (e1 == hle1)
						continue;
					Vector3 hleDir = e1.getDirection ();
					if (Vector3.Angle (hle1Dir, hleDir) == 90) {
						// The other edge used is perpendicular
						hle2 = e1;
						break;
					}
				}
				if (hle2 == null) {
					Debug.Log ("Failed to fill region, no perp. edges");
					return;
				}

				// Use these points to fill
				List<Vector3> points = new List<Vector3>();
				points.Add (hle1.getFrom().getVertex().getPoint());
				points.Add (hle1.getTo().getVertex().getPoint());

				points.Add(hle2.getFrom().getVertex().getPoint());
				points.Add(hle2.getTo().getVertex().getPoint());

				Vector3 dir1 = hle1.getDirection ();
				Vector3 dir2 = hle2.getDirection ();

				// Transform to local space
				dir1 = parent.TransformVector (dir1);
				dir2 = parent.TransformVector (dir2);

				bool horizontalPlane = false;
				if (dir1.y == dir2.y) {
					horizontalPlane = true;
				}

				Vector3 sharedPoint;
				if (points [0] == points [2] || points [0] == points [3]) {
					sharedPoint = points [0];
				} else if (points [1] == points [2] || points [1] == points [3]) {
					sharedPoint = points [1];
				} else {
					Debug.Log ("Warning - no shared points for edges, can't draw region!");
					return;
				}

				sharedPoint = parent.TransformPoint (sharedPoint);

				if (!horizontalPlane) {
					if (dir1.y == 0)
						fillVerticalPlane (dir1, dir2, sharedPoint, r);
					else
						fillVerticalPlane (dir2, dir1, sharedPoint, r);
				} else {
					if (dir1.x > 0)
						fillHorizontalPlane (dir1, dir2, sharedPoint, r);
					else
						fillHorizontalPlane (dir2, dir1, sharedPoint, r);
				}
			}

			public void fillHorizontalPlane(Vector3 dir1, Vector3 dir2, Vector3 sharedPoint, Region r) {
				float actualXSize = dir1.magnitude - 0.1f * 2; // Minus size of edges
				float actualZSize = dir2.magnitude - 0.1f * 2; // Minus size of edges

				int actualX = (int) Math.Floor(actualXSize / cellTargetWidth);
				int actualZ = (int) Math.Floor(actualZSize / cellTargetWidth);

				float actualXNodeSize = actualXSize / actualX;
				float actualZNodeSize = actualZSize / actualZ;

				Vector3 scale = new Vector3 (actualZNodeSize - cellPadding,
					cellTargetHeight - cellPadding, 
					actualXNodeSize - cellPadding);
				scale = div(scale, parent.lossyScale);

				dir1.Normalize ();
				dir2.Normalize ();

				for (int i = 0; i < actualZ; i++) {
					for (int j = 0; j < actualX; j++) {
						Vector3 position = sharedPoint +
							(j * (actualXNodeSize) + actualXNodeSize / 2) * dir1 +
							(i * (actualZNodeSize) + actualZNodeSize / 2) * dir2 +
							0.1f * dir1 + 0.1f * dir2;

						position -= (r.getNormal ().normalized) * (cellTargetHeight/2);

						Cell c = new Cell (parent, position, scale, Quaternion.identity, "Region");
						c.setColor (Color.blue);
					}
				}
			}

			public void fillVerticalPlane(Vector3 horizontalDir, Vector3 verticalDir, Vector3 sharedPoint, Region r) {
				float actualHeight = verticalDir.magnitude - 0.1f * 2; // Minus size of edges
				float actualWidth = horizontalDir.magnitude - 0.1f * 2; // Minus size of edges

				int actualNodeInVert = (int) Math.Floor(actualHeight / cellTargetHeight);
				int actualNodeInHorz = (int) Math.Floor(actualWidth / cellTargetWidth);

				float actualNodeHeight = actualHeight / actualNodeInVert;
				float actualNodeWidth = actualWidth / actualNodeInHorz;

				Vector3 scale = new Vector3 (actualNodeWidth - cellPadding,
					actualNodeHeight - cellPadding, 
					actualNodeWidth - cellPadding);
				scale = div(scale, parent.lossyScale);

				horizontalDir.Normalize ();
				verticalDir.Normalize ();

				for (int i = 0; i < actualNodeInVert; i++) {
					for (int j = 0; j < actualNodeInHorz; j++) {
						Vector3 position = sharedPoint +
							(j * (actualNodeWidth) + actualNodeWidth / 2) * horizontalDir +
							(i * (actualNodeHeight)+ actualNodeHeight / 2) * verticalDir + 
							0.1f * verticalDir + 0.1f * horizontalDir;

						position -= (r.getNormal ().normalized) * (actualNodeWidth/2);

						Cell c = new Cell (parent, position, scale, Quaternion.identity, "Region");
						c.setColor (Color.blue);
					}
				}
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
			BlueCell regionModule = new BlueCell (parent);
			foreach (Corner c in mesh.getCorners()) {
				cornerModule.apply (c);
			}
			foreach (HighLevelEdge e in mesh.getEdges()) {
				edgeModule.apply (e);
			}
			foreach (Region r in mesh.getRegions()) {
				regionModule.apply (r);
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

