using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class HighLevelMesh
	{
		private List<Corner> corners;
		private List<HighLevelEdge> edges;
		private List<Region> regions;
		private MeshStructure sourceMesh;

		public HighLevelMesh (MeshStructure structure) {
			sourceMesh = structure; // Labeled source mesh
			edges = new List<HighLevelEdge> ();
			regions = new List<Region> ();
			corners = new List<Corner> ();
		}

		public void construct() {
			foreach (Vertex v in sourceMesh.getCornerVertices()) {
				Corner c = new Corner (v);
				corners.Add (c);
				v.setParent (c);
			}

			foreach (Corner c in corners) {
				List<HighLevelEdge> edgeResult = constructHLEdges (c);
				edges.AddRange(edgeResult);
			}

			Debug.Log ("We have: " + edges.Count + " HLEs..");
			createRegionsEdges (edges);

			Debug.Log ("Started with: " + regions.Count + " regions..");
			while (mergeRegions ()) {
				// Merge until done!
			}
			Debug.Log ("Ended with: " + regions.Count + " regions..");
		}

		public void createRegionsEdges(List<HighLevelEdge> edges) {
			foreach (HighLevelEdge hle in edges) {
				createRegionsForEdge (hle);
			}
		}

		public bool mergeRegions() {
			foreach (Region r1 in regions) {
				foreach (Region r2 in regions) {
					if (r1 != r2 && r1.attemptMerge (r2)) {
						// We found another merge
						regions.Remove (r2);
						return true;
					}
				}
			}
			// No more merges possible
			return false;
		}

		public void createRegionsForEdge(HighLevelEdge hle) {
			Region r1 = new Region ();
			Region r2 = new Region ();
			foreach (Edge e in hle.getEdges()) {
				foreach (Face f in e.getFaces()) {
					// Attempt to add to both regions
					if (r1.addFace (f)) {
						f.setParent (r1);
						r1.addEdge (hle);
						hle.addRegion (r1);
					} else if (r2.addFace (f)) {
						f.setParent (r2);
						r2.addEdge (hle);
						hle.addRegion (r2);
					}
				}
			}
			regions.Add (r1);
			regions.Add (r2);
		}

		public List<HighLevelEdge> constructHLEdges(Corner source) {
			Vertex.VertexLabel vertexLabel = source.getVertex().getLabel ();
			if (vertexLabel == Vertex.VertexLabel.cornerConvex) {
				// Convex edge
				return constructHLEdgeConvex (source);
			} else {
				// Concave edge
				return constructHLEdgeConcave (source);
			}
		}

		public List<HighLevelEdge> constructHLEdgeConcave(Corner source) {
			return constructHLEdgeConcave (source, new List<Edge> ());
		}

		public List<HighLevelEdge> constructHLEdgeConvex(Corner source) {
			return constructHLEdgeConvex (source, new List<Edge> ());
		}

		public List<HighLevelEdge> constructHLEdgeConcave(Corner source, List<Edge> edges) {
			Vertex sourceVertex = source.getVertex ();
			List<HighLevelEdge> hlEdges = new List<HighLevelEdge> ();
			foreach (Edge e in sourceVertex.getEdges()) {
				if (e.hasParent())
					continue;
				Edge.EdgeLabel edgeLabel = e.getLabel ();
				Vertex target = e.getTo () == sourceVertex ? e.getFrom() : e.getTo();

				if (edgeLabel == Edge.EdgeLabel.concave) {
					// Get a direction vector for this label
					Vector3 direction = (target.getPoint () - sourceVertex.getPoint ()).normalized;

					// Process this edge
					HighLevelEdge hle = processSingleDirectionEdge(edgeLabel, direction, source);
					if (hle != null) {
						hlEdges.Add(hle);
					}
				}
			}
			return hlEdges;
		}

		public List<HighLevelEdge> constructHLEdgeConvex(Corner source, List<Edge> edges) {
			Vertex sourceVertex = source.getVertex ();
			List<HighLevelEdge> hlEdges = new List<HighLevelEdge> ();
			foreach (Edge e in sourceVertex.getEdges()) {
				if (e.hasParent())
					continue;
				Edge.EdgeLabel edgeLabel = e.getLabel ();
				Vertex target = e.getTo () == sourceVertex ? e.getFrom() : e.getTo();

				if (edgeLabel == Edge.EdgeLabel.convex) {
					// Get a direction vector for this label
					Vector3 direction = (target.getPoint () - sourceVertex.getPoint ()).normalized;
					// Process this edge
					HighLevelEdge hle = processSingleDirectionEdge(edgeLabel, direction, source);
					if (hle != null) {
						hlEdges.Add (hle);
					} 
				}
			}
			return hlEdges;
		}


		public HighLevelEdge processSingleDirectionEdge(Edge.EdgeLabel eLabel, Vector3 direction, Corner source) {
			bool changed = true;
			Vertex sourceVertex = source.getVertex ();
			Vertex current = sourceVertex;
			List<Edge> usedEdges = new List<Edge> ();
			while (changed) {
				changed = false;
				foreach (Edge e in current.getEdges()) {
					if (e.hasParent()) {
						continue;
					}
					if (e.getLabel() != eLabel) {
						// If this isn't a correct type of edge, go to the next
						continue;
					}
					// Get our target
					Vertex target = e.getTo () == sourceVertex ? e.getFrom () : e.getTo ();
					// Calculate direction
					Vector3 newDirection = (target.getPoint () - current.getPoint ()).normalized;

					// If the direction isn't the same, we're done
					if (direction != newDirection) {
						continue;
					}

					usedEdges.Add (e);
					// Get the target label
					Vertex.VertexLabel targetLabel = target.getLabel ();
					if (targetLabel == Vertex.VertexLabel.cornerConcave ||
						targetLabel == Vertex.VertexLabel.cornerConvex ||
						targetLabel == Vertex.VertexLabel.cornerSaddle) {
						// Create HLE, this is the end of our HLE
						HighLevelEdge hle = new HighLevelEdge();
						hle.addEdgeRange (usedEdges);
						// Get target corner
						Corner c = target.getParent();
						// Set from and to of hle
						hle.setFrom (source);
						hle.setTo (c);

						// Add edges to the corners
						c.addEdge (hle);
						source.addEdge (hle);

						// Set all edges to processed
						foreach (Edge en in usedEdges) {
							en.setParent (hle);
						}
						return hle;
					} else {
						Debug.Log ("next edge in sequence!");
						current = target;
						changed = true;
					}
				}
			}
			// No valid HLE :(
			return null;
		}

		public List<Corner> getCorners() {
			return corners;
		}

		public List<HighLevelEdge> getEdges() {
			return edges;
		}

		public List<Region> getRegions() {
			return regions;
		}
	}
}

