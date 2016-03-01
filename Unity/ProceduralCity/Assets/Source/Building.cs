using UnityEngine;
using System.Collections;
using ProceduralCity;

public class Building : MonoBehaviour
{
	public long buildingId;
	void Start() {
		OsmBuilding building = Data.Instance.buildings [buildingId];
		//float[] points = building.getPolygon ();
		Vector3[] points = building.getPolygonAsVector3 ();
		Vector2[] meshPoints = new Vector2[points.Length];
		for (int i = 0; i < meshPoints.Length; i++) {
			meshPoints [i] = new Vector2 (points [i].x, points [i].z);
		}
		Triangulator tr = new Triangulator (meshPoints);
		int[] indices = tr.Triangulate ();

		int[] indicesReverse = new int[indices.Length];
		int j = 0;
		for (int i = indices.Length -1; i >= 0; i--) {
			indicesReverse [j] = indices [i];
			j++;
		}

		int[] topAndBottom = new int[indices.Length*2];
		Vector3[] topAndBottomPoints = new Vector3[points.Length * 2];

		for (int i = 0; i < points.Length; i++) {
			topAndBottomPoints [i] = points [i];
			topAndBottomPoints [points.Length + i] = new Vector3 (points [i].x, 10, points [i].z);
			Debug.DrawLine (points [i], topAndBottomPoints [points.Length + i], Color.red, 2000f);
		}
		for (int i = 0; i < indices.Length; i++) {
			topAndBottom [i] = indicesReverse [i];
			topAndBottom [indices.Length + i] = indices [i] + points.Length;
		}
		int[] topAndBottomAndSides = new int[topAndBottom.Length + 6 * points.Length];
		for (int i = 0; i < topAndBottom.Length; i++) {
			topAndBottomAndSides [i] = topAndBottom [i];
		}
		for (int i = 0; i < points.Length; i++) {
				topAndBottomAndSides [topAndBottom.Length + i * 6] = i;
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 4] = i;
			if (i != points.Length - 1) {
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 1] = points.Length + i + 1;
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 2] = points.Length+ i;
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 3] = points.Length + i + 1;
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 5] = i+1;
			} else {
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 1] = points.Length;
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 2] = points.Length+ i;
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 3] = points.Length;
				topAndBottomAndSides [topAndBottom.Length + i * 6 + 5] = 0;
			}
		}
		//for(int i = topAndBottom.Length

		// Create the mesh
		Mesh msh = new Mesh();
		msh.name = "Building";
		msh.vertices = topAndBottomPoints;
		msh.triangles = topAndBottomAndSides;
		msh.RecalculateNormals();
		msh.RecalculateBounds();

		Vector2[] uvs = new Vector2[topAndBottomPoints.Length];
		Bounds bounds = msh.bounds;
		for(int i = 0; i < topAndBottomPoints.Length; i++) {
			uvs[i] = new Vector2(topAndBottomPoints[i].x / bounds.size.x, topAndBottomPoints[i].z / bounds.size.x);
		}
		msh.uv = uvs;

		GameObject meshObject = new GameObject ();
		meshObject.AddComponent<MeshFilter> ().mesh = msh;
		meshObject.AddComponent<MeshRenderer> ();
		meshObject.transform.parent = this.transform;

	}


	void StartOld() {
		OsmBuilding building = Data.Instance.buildings [buildingId];
		//float[] points = building.getPolygon ();
		Vector3[] points = building.getPolygonAsVector3 ();
		var tess = new LibTessDotNet.Tess();

		//Debug.Log ("-"  + buildingId);
		var contour = new LibTessDotNet.ContourVertex[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			// NOTE : Z is here for convenience if you want to keep a 3D vertex position throughout the tessellation process but only X and Y are important.
			contour[i].Position = new LibTessDotNet.Vec3 { X = points[i].x, Y = points[i].y, Z = points[i].z };
			// Data can contain any per-vertex data, here a constant color.
			contour[i].Data = Color.cyan;
			if (i != points.Length-1) {
				Debug.DrawLine(points [i], points [i + 1],Color.red,2000f);
			}

		}
		tess.AddContour(contour, LibTessDotNet.ContourOrientation.Original);
		tess.Tessellate(LibTessDotNet.WindingRule.EvenOdd, LibTessDotNet.ElementType.Polygons, 3, VertexCombine);

		//gameObject.AddComponent<MeshFilter>();
		//gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh ();
		mesh.Clear();
		mesh.name = "Building";
		mesh.vertices = points;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		Vector2[] uvs = new Vector2[points.Length];
		Bounds bounds = mesh.bounds;
		for(int i = 0; i < points.Length; i++) {
			uvs[i] = new Vector2(points[i].x / bounds.size.x, points[i].z / bounds.size.x);
		}
		mesh.uv = uvs;
		mesh.triangles = tess.Elements;
	
		GameObject meshObject = new GameObject ();
		meshObject.AddComponent<MeshFilter> ().mesh = mesh;
		meshObject.AddComponent<MeshRenderer> ();
		meshObject.transform.parent = this.transform;
		//meshObject.transform.position = new Vector3(-mesh.bounds.center.x, -mesh.bounds.center.y, -mesh.bounds.center.z);

		//gameObject.transform.position = new Vector3(-mesh.bounds.center.x, -mesh.bounds.center.y, -mesh.bounds.center.z);
		//GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("Source/Red");
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	// The data array contains 4 values, it's the associated data of the vertices that resulted in an intersection.
	private static object VertexCombine(LibTessDotNet.Vec3 position, object[] data, float[] weights)
	{
		// Fetch the vertex data.
		var colors = new Color[] { (Color)data[0], (Color)data[1], (Color)data[2], (Color)data[3] };
		// Interpolate with the 4 weights.
		var rgba = new float[] {
			(float)colors[0].r * weights[0] + (float)colors[1].r * weights[1] + (float)colors[2].r * weights[2] + (float)colors[3].r * weights[3],
			(float)colors[0].g * weights[0] + (float)colors[1].g * weights[1] + (float)colors[2].g * weights[2] + (float)colors[3].g * weights[3],
			(float)colors[0].b * weights[0] + (float)colors[1].b * weights[1] + (float)colors[2].b * weights[2] + (float)colors[3].b * weights[3],
			(float)colors[0].a * weights[0] + (float)colors[1].a * weights[1] + (float)colors[2].a * weights[2] + (float)colors[3].a * weights[3]
		};
		// Return interpolated data for the new vertex.
		return new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
	}
}

