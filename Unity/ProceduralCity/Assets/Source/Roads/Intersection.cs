using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intersection : MonoBehaviour {

	public List<Road> intersectedRoads;

	// Use this for initialization
	void Start () {
		// Create mesh
		GameObject meshObject = new GameObject ();
		meshObject.transform.parent = this.transform;

		meshObject.name = "Intersection";
		MeshFilter mf = meshObject.AddComponent<MeshFilter> ();
		Renderer renderer = meshObject.AddComponent<MeshRenderer> ();
		Mesh mesh = mf.mesh;
		mesh.Clear ();

//		// Calculate mesh stuff
//		Vector3[] newVertices = new Vector3[points.Count * 2];
//		Vector2[] newUV = new Vector2[newVertices.Length];
//		int[] newTriangles = new int[(newVertices.Length - 2) * 3]; 
//		Bounds bounds = mesh.bounds;
//
//		//		for (int i = 0; i < points.Count - 1; i++) {
//		//			Debug.DrawLine (points[i], points[i+1], Color.red, 200, false);
//		//		}
//
//		float width = 2.0f;
//		for (int i = 0; i < points.Count; i++) {
//			Vector3 p1 = points [i];
//			Vector3 p0 = i == 0 ? 
//				points[i] : points [i - 1];
//
//			Vector3 p2 = i == points.Count - 1 ? 
//				points[i] : points [i + 1];
//
//			Vector3 v1 = CreateVertex (p1, p2, p0, -width / 2);
//			Vector3 v2 = CreateVertex (p1, p2, p0, width / 2);
//
//			newVertices [i * 2] = v1;
//			newVertices [i * 2 + 1] = v2;
//		}
//
//		for (int i = 0; i < newVertices.Length - 2; i+=2) {
//			newTriangles[i / 2 * 6] = i;
//			newTriangles [i / 2 * 6 + 1] = i + 1; 
//			newTriangles [i / 2 * 6 + 2] = i + 2;
//			newTriangles [i / 2 * 6 + 3] = i + 3;
//			newTriangles [i / 2 * 6 + 4] = i + 2;
//			newTriangles [i / 2 * 6 + 5] = i + 1;
//		}
//
//		for (int i = 0; i < newUV.Length; i++) {
//			newUV[i] = new Vector2(newVertices[i].x, newVertices[i].z);
//		}
//
//
//		// Set mesh properties
//		mesh.name = "IntersectionMesh";
//		mesh.vertices = newVertices;
//		mesh.uv = newUV;
//		mesh.triangles = newTriangles;
//
//		mesh.RecalculateNormals();
//		mesh.RecalculateBounds();
//
//		Material newMat = Resources.Load("Materials/Road") as Material;
//		renderer.material = newMat;
	}

	// Update is called once per frame
	void Update () {
	}

	Vector3 CreateVertex(Vector3 pivot, Vector3 v1, Vector3 v2, float offsetX) {
		Vector3 d1 = v1 - pivot;
		Vector3 d2 = v2 - pivot;
		if (d1.magnitude == 0) {
			d1 = Quaternion.Euler (new Vector3(0, 179.0f, 0)) * d2;
		}
		if (d2.magnitude == 0) {
			d2 = Quaternion.Euler(new Vector3(0, 179.0f, 0)) * d1;
		}

		Vector3 result = (d1.normalized + d2.normalized).normalized * offsetX;
		Vector3 cross = Vector3.Cross (d1, d2).normalized;
		//		Debug.DrawRay(pivot, cross, Color.grey, 200, false);

		//		Debug.DrawRay (pivot, d1.normalized, Color.cyan, 200, false);
		//		Debug.DrawRay (pivot, d2.normalized, Color.blue, 200, false);
		//		Debug.DrawRay (pivot, result, Color.yellow, 200);
		if (cross.y > 0) {
			return pivot - result;
		} else {
			return pivot + result;
		}
	}
}