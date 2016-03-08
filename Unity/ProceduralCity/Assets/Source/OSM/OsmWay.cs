using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public class OsmWay : OsmEntity
	{
		public OsmWay (long id, List<OsmTag> tags, List<OsmNodeReference> nodes) : base(id, tags)
		{
			this.nodes = nodes;
		}

		private List<OsmNodeReference> nodes;

		public int getNumberOfNodes() {
			return nodes.Count;
		}

		public long getNodeId(int n) {
			return nodes [n].getId();
		}

		public OsmNodeReference getNodeReference(int n) {
			return nodes [n];
		}

		public Vector3[] getPolygonAsVector3(float y = 0f) {
			Vector3[] points = new Vector3[this.getNumberOfNodes()-1];
			for (int i = 0; i < this.getNumberOfNodes ()-1; i++) {
				OsmNodeReference nodeRef = this.getNodeReference (i);
				float lat = nodeRef.getLattitude();
				float lon = nodeRef.getLongitude();
				points [i] = Map.getVectorFromOrigin (lat, lon, y);
			}
			if(!isClockwise(points) && !isClockWise1(points)) {
				
			}
			//if(!isClockwise(points)) {
			//	points = reverseArray<Vector3> (points);
			//}
			return points;
		}

		public GameObject getMesh(Vector3[] points, string gameObjectName = "Mesh") {
			GameObject g = new GameObject (gameObjectName);
			Vector3 perpendicularVector = this.calculateNormal (points);
			perpendicularVector.Normalize ();
			Vector2[] meshPoints = projectPointsOnPlane(perpendicularVector, points);
			//Debug.Log (isClockwise (meshPoints));
			/*for (int i = 0; i < meshPoints.Length; i++) {
				meshPoints [i] = new Vector2 (points [i].x, points [i].z);
			}*/
			Triangulator tr = new Triangulator (meshPoints);
			int[] indices = tr.Triangulate ();

			int[] indicesReverse = new int[indices.Length];
			int j = 0;
			for (int i = indices.Length -1; i >= 0; i--) {
				indicesReverse [j] = indices [i];
				j++;
			}

			// Create the mesh
			Mesh msh = new Mesh();
			msh.name = "Mesh";
			msh.vertices = points;
			msh.triangles = indices;
			msh.RecalculateNormals();
			msh.RecalculateBounds();

			Vector2[] uvs = new Vector2[points.Length];
			Bounds bounds = msh.bounds;
			for(int i = 0; i < points.Length; i++) {
				uvs[i] = new Vector2(points[i].x / bounds.size.x, points[i].z / bounds.size.x);
			}
			msh.uv = uvs;

			for (int i = 0; i < indices.Length; i = i + 3) {
				if (msh.normals [indices [i / 3]] == Vector3.down) {
					msh.triangles = reverseArray<int>(msh.triangles);
					msh.RecalculateNormals ();
					msh.RecalculateBounds ();
					Debug.Log ("Rotated grid");
					break;
				}
			}
			g.AddComponent<MeshFilter> ().mesh = msh;
			//g.AddComponent<MeshRenderer> ();
			return g;
		}
			
		/// <summary>
		/// Creates the simple mesh.
		/// </summary>
		/// <returns>The simple mesh.</returns>
		/// <param name="points">Points.</param> Maximum of 4 points, ORDER: Top left, top right, bottom right, bottom left
		/// <param name="gameObjectName">Game object name.</param>
		public GameObject createSimpleMesh(Vector3[] points, int[] pointIndices, Vector3 meshCenter, string gameObjectName = "Mesh") {

			if (points.Length > 4) {
				Debug.Log ("Maximaal 4 punten hierin!");
			}
			GameObject g = new GameObject (gameObjectName);
			Output script = g.AddComponent<Output> ();
			Vector3 perpendicularVector = this.calculateNormal (points);
			perpendicularVector.Normalize ();

			int[] indices = new int[6];
			indices [0] = pointIndices[2];
			indices [1] = pointIndices[1];
			indices [2] = pointIndices[0];
			indices [3] = pointIndices[3];
			indices [4] = pointIndices[2];
			indices [5] = pointIndices[0];
		

			// Create the mesh
			Mesh msh = new Mesh();
			msh.name = "Mesh";
			msh.vertices = points;
			msh.triangles = indices;
			msh.RecalculateNormals();
			msh.RecalculateBounds();
			for (int i = 0; i < indices.Length; i = i + 3) {
				Vector3 average = (points [indices [i]] + points [indices [i + 1]] + points [indices [i + 2]]) / 3;
				Debug.DrawRay (average, msh.normals[i/3], Color.cyan, 2000f);
			}
			//Debug.DrawLine (msh.bounds.center, msh.normals[0].normalized, Color.cyan, 2000f);

			Vector2[] uvs = new Vector2[points.Length];
			Bounds bounds = msh.bounds;
			for(int i = 0; i < points.Length; i++) {
				uvs[i] = new Vector2(points[i].x / bounds.size.x, points[i].z / bounds.size.x);
			}
			msh.uv = uvs;
			int numberOfWrongs = 0;
			for (int i = 0; i < indices.Length; i = i + 3) {
				Vector3 center = (points[indices[i]] + points[indices[i+1]] + points[indices[i+2]])/3;
				float angle = Vector3.Dot (msh.normals [i / 3].normalized, (meshCenter - center).normalized);
				//Debug.Log (angle);
				if (angle > 0) {
					script.numberOfWrongs++;
				}
				script.numberOfTriangles++;


			}

			g.AddComponent<MeshFilter> ().mesh = msh;
			//g.AddComponent<MeshRenderer> ();
			return g;
		}

		/*public CombineInstance[] combineMeshes(MeshFilter[] meshFilters) {
			CombineInstance[] combine = new CombineInstance[meshFilters.Length];
			for (int i = 0; i < meshFilters.Length; i++) {
				combine [i].mesh = meshFilters [i].sharedMesh;
				combine [i].transform = meshFilters [i].transform.localToWorldMatrix;
				meshFilters [i].gameObject.SetActive (false);
			}
			return combine;
		}*/

		private Vector3 calculateNormal(Vector3[] points) {
			if (points.Length > 2) {
				Vector3 side1 = points [2] - points [0];
				Vector3 side2 = points [1] - points [0];
				return Vector3.Cross (side1, side2);
			} else {
				throw new IndexOutOfRangeException ("Not enough points on mesh");
			}
		}

		private float calculateAngleWithUpVector(Vector3 normal) {
			return Vector3.Angle (normal, Vector3.up);
		}

		private Vector2[] projectPointsOnPlane(Vector3 normal, Vector3[] points) {
			Vector2[] projectedPoints = new Vector2[points.Length];
			for(int i =0; i < points.Length; i++) {
				Vector3 direction = points[i] - points [0];
				direction = Quaternion.FromToRotation(normal,Vector3.up) * direction;
				Vector3 point = direction + points [0];
				projectedPoints [i] = new Vector2 (point.x, point.z);
			}
			return projectedPoints;
		}

		/// <summary>
		/// Ises the clockwise.
		/// </summary>
		/// <returns><c>true</c>, if clockwise was ised, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public bool isClockwise(Vector3[] points) {
			int l = points.Length ;

			float sum = 0f ;

			for(int i = 0 ; i < l ; i++)
			{
				int n = i+1 >= l-1 ? 0 : i+1 ;

				float x = points[n].x - points[i].x ;
				float y = points[n].z + points[i].z ;
				sum += (x*y) ;
			}

			return (sum < 0) ? false : true;
		}

		public bool isClockWise1(Vector3[] points) {
			{
				int n = points.Length;
				int i,j,k;
				int count = 0;
				double z;

				if (n < 3)
					throw new Exception ("is niks1");
				
				for (i=0;i<n;i++) {
					j = (i + 1) % n;
					k = (i + 2) % n;
					z  = (points[j].x - points[i].x) * (points[k].z - points[j].z);
					z -= (points[j].z - points[i].z) * (points[k].x - points[j].x);
					if (z < 0)
						count--;
					else if (z > 0)
						count++;
				}
				if (count > 0)
					return false;
				else if (count < 0)
					return false;
				else
					throw new Exception ("is niks");
			}
		}

		public T[] reverseArray<T>(T[] a) {
			T[] aReverse = new T[a.Length];
			int j = 0;
			for (int i = a.Length -1; i >= 0; i--) {
				aReverse [j] = a [i];
				j++;
			}
			return aReverse;

		}
			
	}

			
}

