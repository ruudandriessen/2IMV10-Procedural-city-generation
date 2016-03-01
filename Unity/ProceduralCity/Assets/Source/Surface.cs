﻿using UnityEngine;
using System.Collections;
using ProceduralCity;

public class Surface : MonoBehaviour
{
	public long surfaceId;
	void Start() {
		OsmSurface surface = Data.Instance.surfaces [surfaceId];
		Vector3[] points = surface.getPolygonAsVector3 ();
		Vector2[] meshPoints = new Vector2[points.Length];
		for (int i = 0; i < meshPoints.Length; i++) {
			meshPoints [i] = new Vector2 (points [i].x, points [i].z);
		}
		Triangulator tr = new Triangulator (meshPoints);
		int[] indices = tr.Triangulate ();

		int[] indicesReverse = new int[indices.Length];
		int j = 0;
		for (int i = indices.Length - 1; i >= 0; i--) {
			indicesReverse [j] = indices [i];
			j++;
		}

		// Create the mesh
		Mesh msh = new Mesh();
		msh.name = "Surface";
		msh.vertices = points;
		msh.triangles = indices;

		Vector2[] uvs = new Vector2[points.Length];
		Bounds bounds = msh.bounds;
		for(int i = 0; i < points.Length; i++) {
			uvs[i] = new Vector2(points[i].x / bounds.size.x, points[i].z / bounds.size.x);
		}
		msh.uv = uvs;

		GameObject meshObject = new GameObject ();
		meshObject.AddComponent<MeshFilter> ().mesh = msh;
		Renderer renderer = meshObject.AddComponent<MeshRenderer> ();
		meshObject.transform.parent = this.transform;

		string type = surface.getType ();
		Material newMat;
		switch (type) {
		case "forest":
			newMat = Resources.Load ("Materials/Forest") as Material;
			break;
		case "reservoir":
		case "reservoir:bottom":
		case "water":
		case "coastline":
			newMat = Resources.Load ("Materials/Reservoir") as Material;
			break;
		case "residential":
			newMat = Resources.Load ("Materials/Residential") as Material;
			break;
		case "grass":
		case "meadow":
			newMat = Resources.Load ("Materials/Grass") as Material;
			break;
		case "farmyard":
			newMat = Resources.Load ("Materials/Farmyard") as Material;
			break;
		case "industrial":
			newMat = Resources.Load ("Materials/Industrial") as Material;
			break;
		case "recreation_ground":
			newMat = Resources.Load ("Materials/Recreation") as Material;
			break;
		default:
			newMat = Resources.Load("Materials/House") as Material;
			Debug.Log("Unknown surface type: " + type);
			break;
		}

		newMat.name = type;
		renderer.material = newMat;

		msh.RecalculateNormals();
		msh.RecalculateBounds();
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

