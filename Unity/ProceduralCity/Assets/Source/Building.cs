using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Building : MonoBehaviour
{
	public long buildingId;
	void Start() {
		OsmBuilding building = Data.Instance.buildings [buildingId];
		//float[] points = building.getPolygon ();
		Vector2[] points = building.getPolygonAsVector2 ();
		Vector3[] vectors = new Vector3[points.Length];
		for (int i = 0; i < points.Length; i++) {
			vectors [i] = new Vector3 (points [i].x, 1, points[i].y);
		}
		var tess = new LibTessDotNet.Tess();


		var contour = new LibTessDotNet.ContourVertex[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			// NOTE : Z is here for convenience if you want to keep a 3D vertex position throughout the tessellation process but only X and Y are important.
			contour[i].Position = new LibTessDotNet.Vec3 { X = points[i].x, Y = 0.0f, Z = points[i].y };
			// Data can contain any per-vertex data, here a constant color.
			contour[i].Data = Color.cyan;
		}
		tess.AddContour(contour, LibTessDotNet.ContourOrientation.Original);
		tess.Tessellate(LibTessDotNet.WindingRule.EvenOdd, LibTessDotNet.ElementType.Polygons, 3, VertexCombine);

		//gameObject.AddComponent<MeshFilter>();
		//gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh ();
		mesh.Clear();
		mesh.name = "Building";
		mesh.vertices = vectors;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		Vector2[] uvs = new Vector2[vectors.Length];
		Bounds bounds = mesh.bounds;
		for(int i = 0; i < vectors.Length; i++) {
			uvs[i] = new Vector2(vectors[i].x / bounds.size.x, vectors[i].z / bounds.size.x);
		}
		mesh.uv = uvs;
		mesh.triangles = tess.Elements;
	
		GameObject meshObject = new GameObject ();
		meshObject.AddComponent<MeshFilter> ().mesh = mesh;
		meshObject.AddComponent<MeshRenderer> ();
		meshObject.transform.position = new Vector3(-mesh.bounds.center.x, -mesh.bounds.center.y, -mesh.bounds.center.z);

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

