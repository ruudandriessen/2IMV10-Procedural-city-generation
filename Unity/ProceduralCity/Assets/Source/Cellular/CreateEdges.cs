using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;

public class CreateEdges : MonoBehaviour {
	Dictionary<int, Vertex> indexToVertex = new Dictionary<int, Vertex> ();
	List<Edge> edges = new List<Edge> ();
	Vector3[] vertices;
	Vector3[] normals;
	int[] triangles;

	// Use this for initialization
	void Start () {
		List<Face> faces = new List<Face> ();

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		normals = mesh.normals;
		triangles = mesh.triangles;

		for (int i = 0; i < triangles.Length - 1; i+=3) {
			Face f = createFace (i);
			faces.Add (f);
		}

		for (int i = 0; i < edges.Count; i++) {
			Edge e = edges [i];
			List<Face> edgeFaces = e.getFaces();
			if (edgeFaces.Count < 2) {
				continue;
			}
			Vector3 n1 = edgeFaces[0].getNormal();
			Vector3 n2 = edgeFaces[1].getNormal();

			float angle = Vector3.Angle (n1, n2);
			if (angle == 180 || angle == 0) {
				e.setLabel (Edge.EdgeLabel.flat);
			} else {
				float dot = Vector3.Dot (n1, n2);
				if ( dot < 0 )
					e.setLabel (Edge.EdgeLabel.concave);
				else
					e.setLabel (Edge.EdgeLabel.convex);
			}
		}
	}

	public Face createFace(int i) {
		Vertex v1 = getVertex (i);
		Vertex v2 = getVertex (i + 1);
		Vertex v3 = getVertex (i + 2);

		Vector3 p1 = v1.getPoint ();
		Vector3 p2 = v2.getPoint ();
		Vector3 p3 = v3.getPoint ();

		Vector3 center = (p1 + p2 + p3) / 3;

        Vector3 n1 = normals[triangles[i]];
        Vector3 n2 = normals[triangles[i + 1]];
        Vector3 n3 = normals[triangles[i + 2]];

        Vector3 normal = (n1 + n2 + n3) / 3;

        Face f = new Face (center, normal);

		Edge e1 = new Edge (p1, p2); 
		Edge e2 = new Edge (p2, p3);
		Edge e3 = new Edge (p3, p1);

		if (edges.Contains (e1))
			e1 = edges[edges.IndexOf (e1)];
		if (edges.Contains(e2))
			e2 = edges[edges.IndexOf (e2)];
		if (edges.Contains(e3))
			e3 = edges[edges.IndexOf (e3)];

		f.addVertex (v1);
		f.addVertex (v2);
		f.addVertex (v3);

		f.addEdge (e1);
		f.addEdge (e2);
		f.addEdge (e3);

		e1.addFace (f);
		e2.addFace (f);
		e3.addFace (f);

		edges.Add (e1);
		edges.Add (e2);
		edges.Add (e3);

		return f;
	}

	public Vertex getVertex(int index) {
		if (indexToVertex.ContainsKey (index)) {
			return indexToVertex [index];
		} else {
			return new Vertex (vertices[triangles[index]]);
		}
	}

	// Update is called once per frame
	void Update () {

	}	
	
}