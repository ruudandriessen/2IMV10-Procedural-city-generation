using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;

public class CreateEdges : MonoBehaviour {
	MeshStructure meshStruct;
	HighLevelMesh highMesh;
	Vector3[] vertices;
	Vector3[] normals;
	int[] triangles;

	// Use this for initialization
	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		normals = mesh.normals;
		triangles = mesh.triangles;
		meshStruct = new MeshStructure ();

		for (int i = 0; i < triangles.Length - 1; i+=3) {
			Face f = createFace (i);
			meshStruct.addFace (f);
		}

		meshStruct.recalculateLabels ();

		highMesh = new HighLevelMesh(meshStruct);
		highMesh.construct ();
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

		Edge e1 = new Edge (v1, v2); 
		Edge e2 = new Edge (v2, v3);
		Edge e3 = new Edge (v3, v1);

		e1 = getEdge (e1);
		e2 = getEdge (e2);
		e3 = getEdge (e3);

		f.addVertex (v1);
		f.addVertex (v2);
		f.addVertex (v3);

		f.addEdge (e1);
		f.addEdge (e2);
		f.addEdge (e3);

		v1.addEdge (e1);
		v2.addEdge (e1);
		v2.addEdge (e2);
		v3.addEdge (e2);
		v3.addEdge (e3);
		v1.addEdge (e3);

		e1.addFace (f);
		e2.addFace (f);
		e3.addFace (f);

		meshStruct.addVertex (v1);
		meshStruct.addVertex (v2);
		meshStruct.addVertex (v3);
		meshStruct.addEdge (e1);
		meshStruct.addEdge (e2);
		meshStruct.addEdge (e3);

		return f;
	}

	public Vertex getVertex(int index) {
		int vertexIndex = triangles [index];
		Vertex v = new Vertex (vertices [vertexIndex]);
		int vIndex = meshStruct.indexOfVertex (v);
		if (vIndex == -1) {
			meshStruct.addVertex (v);	
			return v;
		} else {
			return meshStruct.getVertex(vIndex);
		}	
	}

	public Edge getEdge(Edge e) {
		int eIndex = meshStruct.indexOfEdge (e);
		if (eIndex == -1) {
			return e;
		} else {			
			return meshStruct.getEdge (eIndex);
		}
	}

	// Update is called once per frame
	void Update () {

	}	
	
}