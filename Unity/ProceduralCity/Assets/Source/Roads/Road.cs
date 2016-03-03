using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;

public class Road : MonoBehaviour {
	
	public long streetId;
	private List<Vector3> points;
	OsmStreet streetData;

	// Use this for initialization
	void Start () {
		initializeData ();

		// Create mesh and set parent to road
		GameObject meshObject = new GameObject ();
		meshObject.transform.parent = this.transform;
		meshObject.name = "Road";

		MeshFilter mf = meshObject.AddComponent<MeshFilter> ();
		Renderer renderer = meshObject.AddComponent<MeshRenderer> ();
		Mesh mesh = mf.mesh;
		mesh.Clear ();

		// Mesh information
		Vector3[] newVertices = calculateVertices ();
		Vector2[] newUV = new Vector2[newVertices.Length];
		int[] newTriangles = calculateTriangles ();
		Bounds bounds = mesh.bounds;

		for (int i = 0; i < newUV.Length; i++) {
			newUV[i] = new Vector2(newVertices[i].x, newVertices[i].z);
		}

		// Set mesh properties
		mesh.name = "RoadMesh";
		mesh.vertices = newVertices;
		mesh.uv = newUV;
		mesh.triangles = newTriangles;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		Material newMat = Resources.Load("Materials/Road") as Material;
		renderer.material = newMat;
	}

	private void initializeData() {	
		this.streetData = Data.Instance.streets [streetId];
		this.points = loadPoints (streetData);
	}

	private List<Vector3> loadPoints(OsmStreet street) {
		List<Vector3> dataPoints = new List<Vector3> ();
		for (int i = 0; i < street.getNumberOfNodes (); i++) {
			OsmNode n1 = street.getNode(i);
			Vector3 r1 = Map.getVectorFromOrigin 
				(n1.getLatitude(), n1.getLongitude());
			if (n1.amountContainedIn () > 1) {
				Debug.DrawRay (r1, Vector3.up, Color.blue, 200);
			}
			dataPoints.Add (r1);
		}
		return dataPoints;
	}

	private int[] calculateTriangles () {
		int[] triangles = new int[(points.Count * 2 - 2) * 3];
		for (int i = 0; i < points.Count * 2 - 2; i+=2) {
			triangles[i / 2 * 6] = i;
			triangles [i / 2 * 6 + 1] = i + 1; 
			triangles [i / 2 * 6 + 2] = i + 2;
			triangles [i / 2 * 6 + 3] = i + 3;
			triangles [i / 2 * 6 + 4] = i + 2;
			triangles [i / 2 * 6 + 5] = i + 1;
		}
		return triangles;
	}

	private Vector3[] calculateVertices() {
		Vector3[] vectices = new Vector3[points.Count * 2];

		float width = 2.0f;
		for (int i = 0; i < points.Count; i++) {
			Vector3 p1 = points [i];
			Vector3 p0 = i == 0 ? 
				points[i] : points [i - 1];

			Vector3 p2 = i == points.Count - 1 ? 
				points[i] : points [i + 1];

			Vector3 v1 = CreateVertex (p1, p2, p0, -width / 2);
			Vector3 v2 = CreateVertex (p1, p2, p0, width / 2);

			vectices [i * 2] = v1;
			vectices [i * 2 + 1] = v2;
		}

		return vectices;
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

	// Update is called once per frame
	void Update () {
	}
}
