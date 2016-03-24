using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class ComputeOperation : Operation
	{
		string[] outputs;
		string param;
		string type;
		public ComputeOperation (Scope s, string type, string param, string[] outputs) : base(s)
		{
			this.param = param;
			this.type = type;
			this.outputs = outputs;
		}

		public override void applyOperation(Symbol s, ref List<Symbol> symbols) {
			switch (type) {
			case "sidefaces":
				List<Vector3> points = s.getPoints ();
				int numberOfFaces = s.getPoints ().Count / 2;
				Debug.Log ("Sidefaces: " + numberOfFaces);
				Vector3 c = new Vector3 (0, 0, 0);
				int numberOfPoints = points.Count/2;
				for (int i = 0; i < numberOfPoints; i++) {
					c += points [i]/  numberOfPoints;
					//c.y += points [i].y / (float) numberOfPoints;
					//c.z += points [i].z / (float) numberOfPoints;
				}
				for(int i = 0; i < numberOfFaces; i++) {
					Vector3[] sidefacePoints = new Vector3[4];
					sidefacePoints [0] = points [i];
					sidefacePoints [1] = points [(i + 1) % numberOfFaces];
					sidefacePoints [2] = points [numberOfFaces+i];
					sidefacePoints [3] = points [numberOfFaces+(i + 1) % numberOfFaces];
					Symbol newSymbol = new Symbol (this.param, sidefacePoints, true);

					newSymbol.extraValues.Add ("center", c);
					symbols.Add (newSymbol);
				}


				/*Mesh m = scope.getGameObject ().GetComponentInChildren<MeshFilter> ().mesh;
				m.RecalculateBounds ();
				m.RecalculateNormals ();
				Vector3[] vertices = new Vector3[m.vertices.Length / 3];
				HashSet<Vector3> uniqueVertices = new HashSet<Vector3> ();
				for (int i = 0; i < m.triangles.Length; i++) {
					
				}


				for (int i = 0; i < m.vertices.Length; i++) {
					uniqueVertices.Add (m.vertices [i]);
					Debug.Log (scope.getGameObject ().transform.GetChild (0).name);
					Debug.DrawRay (scope.getGameObject ().transform.GetChild (0).transform.localToWorldMatrix.MultiplyPoint (m.vertices [i]), m.normals [i], Color.cyan, 2000f);
					Debug.Log (scope.getGameObject ().transform.GetChild (0).transform.localToWorldMatrix.MultiplyPoint (m.vertices [i]));
				}

				//for (int i = 0; i < points.Length; i++) {
				//	Debug.DrawLine (points [i], points [i] + Vector3.up, Color.red, 2000f);
				//}*/
				break;
			}
		}
	}
}

