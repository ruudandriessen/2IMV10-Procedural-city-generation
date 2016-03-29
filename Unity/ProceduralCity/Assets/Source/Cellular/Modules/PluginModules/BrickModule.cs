//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//namespace ProceduralCity
//{
//	public class BrickModule : ModelModule
//	{
//		/*
//		 * Fills corners with white randomised cells
//		 */
//		class WhiteCorners : CornerModule {
//			Transform parent;
//			public WhiteCorners(Transform parent) : base() {
//				this.parent = parent;
//			}
//
//			float size = 0.1f; 
//			float cellPadding = 0.005f;
//
//			public override bool apply(Corner corner) {
//				Vector3 p = corner.getVertex ().getPoint();
//				p = parent.TransformPoint (p);
//
//				Vector3 translationDirection = Vector3.zero;
//				int count = 0;
//				foreach (HighLevelEdge e in corner.getEdges()) {
//					foreach (Region r in e.getRegions()) {
//						p -= (r.getNormal ().normalized) * (size / 4);
//					}
//				}
//
//				Vector3 scale = new Vector3 (size - cellPadding, size - cellPadding, size - cellPadding);
//				scale = div(scale, parent.lossyScale);
//
//				Cell c = new Cell (parent, p, scale, Quaternion.identity, " Corner");
//
//				float value = UnityEngine.Random.value;
//				Color color = Color.white;
//				value = 0.4f * value;
//				color = Color.Lerp(color, Color.black ,value);
//				c.setColor (color);
//				return true;
//			}
//		}
//
//		/*
//		 * Fills edges with white cells
//		 */
//		class WhiteEdges : EdgeModule {
//			Transform parent;
//			public WhiteEdges(Transform parent) : base() {
//				this.parent = parent;
//			}
//
//			float cellTargetSize = 0.1f;
//			float cellPadding = 0.005f;
//
//			float targetBrickWidth = 0.2f;
//			float targetBrickHeight = 0.05f;
//
//			public override bool apply(HighLevelEdge e) {
//				Corner c1 = e.getFrom (); 
//				Corner c2 = e.getTo ();
//
//				// Get the corner points
//				Vector3 p1 = c1.getVertex ().getPoint ();
//				Vector3 p2 = c2.getVertex ().getPoint ();
//
//				// Transform to local space
//				p1 = parent.TransformPoint (p1);
//				p2 = parent.TransformPoint (p2);
//
//				// Get direction of the edge
//				Vector3 direction = (p1 - p2);
//				float edgeSize = direction.magnitude;
//				edgeSize -= 0.1f * 2; // Minus 2x size of a corner
//				direction.Normalize();
//
//				int estCellCount = (int) Math.Floor(edgeSize / cellTargetSize);
//				float actualCellSize = edgeSize / estCellCount;
//				int actualCellCount = (int) (edgeSize / actualCellSize);
//
//				int actVBrickCount = (int)Math.Floor (edgeSize / targetBrickHeight);
//
//				float actualBrickHeight = edgeSize / actVBrickCount;
//
//				Vector3 scale = new Vector3 (actualCellSize - cellPadding,
//					actualCellSize - cellPadding, 
//					actualCellSize - cellPadding);
//				scale = div(scale, parent.lossyScale);
//
//
//				Vector3 scaleBrickX = new Vector3 (targetBrickWidth - cellPadding,
//					actualBrickHeight - cellPadding, 
//					actualCellSize - cellPadding);
//				scaleBrickX = div(scaleBrickX, parent.lossyScale);
//
//				Vector3 scaleBrickZ = new Vector3 (actualCellSize - cellPadding,
//					actualBrickHeight - cellPadding, 
//					targetBrickWidth - cellPadding);
//				scaleBrickZ = div(scaleBrickZ, parent.lossyScale);
//
//				bool isVertical = false;
//				if (direction.y != 0) {
//					isVertical = true;
//				}
//
//				if (isVertical) {
//					for (int i = 0; i < actVBrickCount; i++) {
//						Vector3 position = p1 - i * direction * actualBrickHeight -
//							1.0f * 0.1f * direction  - // Corner size
//							0.5f * actualCellSize * direction + // Half a cell
//							0.5f * actualBrickHeight * direction;
//						
//						Vector3 xNormal = Vector3.zero;
//						Vector3 zNormal = Vector3.zero;
//						foreach (Region r in e.getRegions()) {
//							Vector3 normal = r.getNormal ().normalized;
//							if (normal.x != 0) {
//								xNormal = normal;
//								if (i % 2 == 0) {
//									position -= normal * (actualCellSize/2 - cellPadding/4);
//								}
//							} else if (normal.z != 0) {
//								zNormal = normal;
//								if (i % 2 == 1) {
//									position -= normal * (actualCellSize/2 - cellPadding/4);
//								}
//							} 
//						}
//						Cell c = null;
//						if (i % 2 == 0) {
//							position -= zNormal * targetBrickWidth/2;
//							c = new Cell (parent, position, scaleBrickZ, Quaternion.identity, "Edge");
//						} else {
//							position -= xNormal * targetBrickWidth/2;
//							c = new Cell (parent, position, scaleBrickX, Quaternion.identity, "Edge");
//						}
//						
//						float value = UnityEngine.Random.value;
//						value = 0.8f * value;
//						Color color = Color.red;
//						color = Color.Lerp(color, Color.black ,value);
//						c.setColor (color);
//					}
//				} else {
//					for (int i = 0; i < actualCellCount; i++) {
//						Vector3 position = p1 - i * direction * actualCellSize -
//							1.0f * 0.1f * direction - // Corner size
//							0.5f * actualCellSize * direction ; // Half a cell
//						foreach (Region r in e.getRegions()) {
//							Vector3 normal = r.getNormal ().normalized;
//							position -= normal * (actualCellSize/2 - cellPadding/4);
//						}
//						Cell c = new Cell (parent, position, scale, Quaternion.identity, "Edge");
//
//						float value = UnityEngine.Random.value;
//						Color color = Color.white;
//						value = 0.4f * value;
//						color = Color.Lerp(color, Color.black ,value);
//						c.setColor (color);
//					}
//				}
//				return true;
//			}
//		}
//
//		/*
//		 * Fills regions with red tinted bricks
//		 */
//		class BrickRegions : RegionModule {
//			Transform parent;
//			public BrickRegions(Transform parent) : base() {
//				this.parent = parent;
//			}
//
//			float cellTargetWidth = 0.1f;
//			float brickWidth = 0.2f;
//			float cellTargetHeight = 0.05f;
//			float cellPadding = 0.005f;
//
//			public override bool apply(Region r) {
//				HighLevelEdge hle1 = null, hle2 = null;
//
//				// Take one of the edges as a basis
//				hle1 = r.getEdges () [0];
//
//				// Get it's direction
//				Vector3 hle1Dir = hle1.getDirection ();
//				foreach (HighLevelEdge e1 in r.getEdges()) {
//					if (e1 == hle1)
//						continue;
//					Vector3 hleDir = e1.getDirection ();
//					if (Vector3.Angle (hle1Dir, hleDir) == 90) {
//						// The other edge used is perpendicular
//						hle2 = e1;
//						break;
//					}
//				}
//				if (hle2 == null) {
//					Debug.Log ("Failed to fill region, no perp. edges");
//					return false;
//				}
//
//				// Use these points to fill
//				List<Vector3> points = new List<Vector3>();
//				points.Add (hle1.getFrom().getVertex().getPoint());
//				points.Add (hle1.getTo().getVertex().getPoint());
//
//				points.Add(hle2.getFrom().getVertex().getPoint());
//				points.Add(hle2.getTo().getVertex().getPoint());
//
//				Vector3 dir1 = hle1.getDirection ();
//				Vector3 dir2 = hle2.getDirection ();
//
//				// Transform to local space
//				dir1 = parent.TransformVector (dir1);
//				dir2 = parent.TransformVector (dir2);
//
//				bool horizontalPlane = false;
//				if (dir1.y == dir2.y) {
//					horizontalPlane = true;
//				}
//
//				Vector3 sharedPoint;
//				if (points [0] == points [2] || points [0] == points [3]) {
//					sharedPoint = points [0];
//				} else if (points [1] == points [2] || points [1] == points [3]) {
//					sharedPoint = points [1];
//				} else {
//					Debug.Log ("Warning - no shared points for edges, can't draw region!");
//					return false;
//				}
//
//				sharedPoint = parent.TransformPoint (sharedPoint);
//
//				if (!horizontalPlane) {
//					if (dir1.y == 0)
//						fillVerticalPlane (dir1, dir2, sharedPoint, r);
//					else
//						fillVerticalPlane (dir2, dir1, sharedPoint, r);
//				} else {
//					if (dir1.x > 0)
//						fillHorizontalPlane (dir1, dir2, sharedPoint, r);
//					else
//						fillHorizontalPlane (dir2, dir1, sharedPoint, r);
//				}
//				return true;
//			}
//
//			public void fillHorizontalPlane(Vector3 dir1, Vector3 dir2, Vector3 sharedPoint, Region r) {
//				float actualXSize = dir1.magnitude - 0.1f * 2; // Minus size of edges
//				float actualZSize = dir2.magnitude - 0.1f * 2; // Minus size of edges
//
//				int actualX = (int) Math.Floor(actualXSize / cellTargetWidth);
//				int actualZ = (int) Math.Floor(actualZSize / cellTargetWidth);
//
//				float actualXNodeSize = actualXSize / actualX;
//				float actualZNodeSize = actualZSize / actualZ;
//
//				Vector3 scale = new Vector3 (actualZNodeSize - cellPadding,
//					cellTargetHeight - cellPadding, 
//					actualXNodeSize - cellPadding);
//				scale = div(scale, parent.lossyScale);
//
//				dir1.Normalize ();
//				dir2.Normalize ();
//
//				for (int i = 0; i < actualZ; i++) {
//					for (int j = 0; j < actualX; j++) {
//						Vector3 position = sharedPoint +
//							(j * (actualXNodeSize) + actualXNodeSize / 2) * dir1 +
//							(i * (actualZNodeSize) + actualZNodeSize / 2) * dir2 +
//							0.1f * dir1 + 0.1f * dir2;
//
//						position -= (r.getNormal ().normalized) * (cellTargetHeight/2);
//
//						Cell c = new Cell (parent, position, scale, Quaternion.identity, "Region");
//
//						float value = UnityEngine.Random.value;
//						Color color = Color.white;
//						value = 0.4f * value;
//						color = Color.Lerp(color, Color.black ,value);
//						c.setColor (color);
//					}
//				}
//			}
//
//			public void fillVerticalPlane(Vector3 horizontalDir, Vector3 verticalDir, Vector3 sharedPoint, Region r) {
//				float actualHeight = verticalDir.magnitude - 0.1f * 2; // Minus size of edges
//				float actualWidth = horizontalDir.magnitude - 0.1f * 2; // Minus size of edges
//
//				int actualNodeInVert = (int) Math.Floor(actualHeight / cellTargetHeight);
//				int actualNodeInHorz = (int) Math.Floor(actualWidth / brickWidth);
//
//				float actualNodeHeight = actualHeight / actualNodeInVert;
//				float actualNodeWidth = actualWidth / actualNodeInHorz;
//
//				Vector3 scale = new Vector3 (actualNodeWidth - cellPadding,
//					actualNodeHeight - cellPadding, 
//					actualNodeWidth - cellPadding);
//				scale = div(scale, parent.lossyScale);
//
//				Vector3 smallScale = new Vector3 (actualNodeWidth/2 - cellPadding,
//					actualNodeHeight - cellPadding, 
//					actualNodeWidth/2 - cellPadding);
//				smallScale = div(smallScale, parent.lossyScale);
//
//				horizontalDir.Normalize ();
//				verticalDir.Normalize ();
//
//				for (int i = 0; i < actualNodeInVert; i++) {
//					for (int j = 0; j < actualNodeInHorz; j++) {
//						Vector3 position = sharedPoint +
//							(j * (actualNodeWidth) + actualNodeWidth / 2) * horizontalDir +
//							(i * (actualNodeHeight)+ actualNodeHeight / 2) * verticalDir + 
//							0.1f * verticalDir + 0.1f * horizontalDir;
//
//						if (i % 2 == 1) {
//							position += brickWidth/2 * horizontalDir;
//						}
//						
//						position -= (r.getNormal ().normalized) * (actualNodeWidth/2);
//
//						Cell c = new Cell (parent, position, scale, Quaternion.identity, "Region");
//						float value = UnityEngine.Random.value;
//						Color brickColor = Color.red;
//						value = 0.2f * value;
//						brickColor = Color.Lerp (brickColor, Color.black, value);
//						c.setColor (brickColor);
//					}
//				}
//			}
//		}
//
//		Transform parent;
//		public BrickModule (Transform parent) : base()
//		{
//			this.parent = parent;
//		}
//
//		public override bool apply(HighLevelMesh mesh) { 
//			WhiteCorners cornerModule = new WhiteCorners (parent);
//			WhiteEdges edgeModule = new WhiteEdges (parent);
//			BrickRegions regionModule = new BrickRegions (parent);
//			foreach (Corner c in mesh.getCorners()) {
//				cornerModule.apply (c);
//			}
//			foreach (HighLevelEdge e in mesh.getEdges()) {
//				edgeModule.apply (e);
//			}
//			foreach (Region r in mesh.getRegions()) {
//				regionModule.apply (r);
//			}
//			return true;
//		}
//
//		static public Vector3 div(Vector3 v1, Vector3 v2) {
//			v1.x /= v2.x;
//			v1.y /= v2.y;
//			v1.z /= v2.z;
//			return v1;
//		}
//	}
//}
//
