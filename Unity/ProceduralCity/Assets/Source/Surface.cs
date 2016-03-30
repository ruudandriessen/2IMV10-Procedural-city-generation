using UnityEngine;
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

		GameObject meshObject = new GameObject ("SurfaceMesh");
		meshObject.isStatic = true;
		meshObject.transform.parent = this.transform;
		meshObject.transform.Translate(new Vector3(0, 0.02f, 0));

		string type = surface.getType ();
		Material newMat;
		switch (type) {
		case "reservoir":
		case "reservoir:bottom":
		case "water":
		case "coastline":
			newMat = Resources.Load ("Materials/Reservoir") as Material;
			break;
		case "residential":
			newMat = Resources.Load ("Materials/Pavement_01", typeof(Material)) as Material;
			meshObject.transform.Translate(new Vector3(0, -0.01f, 0));
			break;
		case "forest":
		case "grass":
		case "meadow":
			newMat = Resources.Load ("Materials/TL_Grass_01") as Material;
			break;
		case "farm":
		case "farmyard":
//			newMat = Resources.Load ("Materials/AQUAS_Lite_Water") as Material;
//			meshObject.AddComponent<AQUAS_Reflection> ();
			return;
			break;
		case "industrial":
			return; //newMat = Resources.Load ("Materials/Industrial") as Material;
			break;
		case "recreation_ground":
			return; // newMat = Resources.Load ("Materials/Recreation") as Material;
			break;
		default:
			newMat = Resources.Load("Materials/Unknown") as Material;
			Debug.Log("Unknown surface type: " + type);
			break;
		}

		meshObject.AddComponent<MeshFilter> ().mesh = msh;
		Renderer renderer = meshObject.AddComponent<MeshRenderer> ();
		newMat.name = type;
		renderer.material = newMat;

		msh.RecalculateNormals();
		msh.RecalculateBounds();
	}



	// Update is called once per frame
	void Update ()
	{
	}
}

