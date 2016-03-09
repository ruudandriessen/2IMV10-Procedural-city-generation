using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;

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

	public void setLabel(Edge.EdgeLabel lbl) {
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

public class Vertex {
	private Vector3 p;
	List<Edge> edges;

	public Vertex (Vector3 p) {
		this.p = p;
		edges = new List<Edge> ();
	}

	public void addEdge(Edge e){ 
		edges.Add (e);
	}

	public Vector3 getPoint() {
		return p;
	}

	public override bool Equals(System.Object obj)
	{
		// Return false if null or if it cannot be cast
		if (obj == null)
			return false;
		Vertex v = obj as Vertex;
		if ((System.Object)v == null)
			return false;
		// If the points are equal, so are the vertices
		return (this == v);
	}

	public static bool operator ==(Vertex v1, Vertex v2) {
		return v1.getPoint () == v2.getPoint ();
	}

	public static bool operator !=(Vertex v1, Vertex v2)
	{
		return !(v1 == v2);
	}
}

public class Face {
	private Vector3 center;
	private Vector3 normal;
	List<Edge> edges;
	List<Vertex> vertices;

	public Face(Vector3 c, Vector3 n) {
		this.center = c;
		this.normal = n;
		this.edges = new List<Edge> ();
		this.vertices = new List<Vertex> ();
	}

	public void addEdge (Edge e) {
		edges.Add (e);
	}	

	public void addVertex (Vertex v) {
		vertices.Add (v);
	}

	public Vector3 getNormal() {
		return normal;
	}

	public Vector3 getCenter() {
		return center;
	}
}

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
		Vector3 normal = normals [i / 3];

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
		
	public static bool VectorsEqual(Vector3 v1, Vector3 v2) {
		if (v1.x != v2.x) {
			return false;
		}
		if (v1.y != v2.y) {
			return false;
		}
		if (v1.z != v2.z) {
			return false;
		}
		return true;
	}
}