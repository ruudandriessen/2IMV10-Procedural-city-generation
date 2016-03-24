using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;

public class HouseGeneration : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Scope s = new Scope (this.gameObject);
		Rule r1 = new Rule ("Footprint")
			.add (new ScaleOperation (s, 1, new System.Random().Next(12, 30), 1))
			.add (new SaveOperation(s, "Facades"));
		Rule r2 = new Rule ("Facades")
			.add (new ComputeOperation (s, "sidefaces", "Facade",
										new string[] {
						
										}));
		Rule r3 = new Rule ("Facade")
			.add (new SubdivideOperation (s, "Y",
										new string[] {
											"1r", "0.5", "3.5"
										},
										new string[] {
											"Floors", "Ledge", "Groundfloor"
										}));
		Rule r4 = new Rule("Floors")
			.add(new RepeatOperation(s, "Y", "3", "Floor"));
		Rule r5 = new Rule ("Floor")
			.add (new RepeatOperation (s, "X", "2", "WindowTile"));
		Rule r6 = new Rule("WindowTile")
			.add(new SubdivideOperation(s, "X",
				new string[] {
					"1r", "0.9", "1r"
				},
				new string[] {
					"Wall", "VerticalWindowTile", "Wall"
				}));
		Rule r7 = new Rule("VerticalWindowTile")
			.add(new SubdivideOperation(s, "Y",
				new string[] {
					"1r", "1.55", "1r"
				},
				new string[] {
					"Wall", "Window", "Wall"
				}));
		Rule r8 = new Rule("Window")
			.add(new InsertModelOperation(s, "window"));
		Rule r9 = new Rule ("Groundfloor")
			.add (new SubdivideOperation (s, "X",
			          new string[] {
				"1r", "1.8", "1r"
			},
			          new string[] {
				"WindowTile", "DoorTile", "WindowTile" 
			}));
		Rule r10 = new Rule ("DoorTile")
			.add (new SubdivideOperation (s, "X",
			           new string[] {
				"1r", "2", "1r"
			},
			           new string[] {
				"Wall", "VerticalDoorTile", "Wall"
			}));
		Rule r11 = new Rule ("VerticalDoorTile")
			.add (new SubdivideOperation (s, "Y",
			           new string[] {
					"1r", "3.5"
			},
			           new string[] {
					"Wall", "Door"
			}));
		Rule r12 = new Rule ("Door")
			.add (new InsertModelOperation (s, "door"));

		//.add (new TranslateOperation (g3, 8 / 2, 15 / 2, 8 / 2))
		//.add (new TranslateOperation (g3, 0, 0, 16));
		Vector3[] footprint = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (10, 0, 0),
			new Vector3 (15, 0, 5),
			new Vector3 (10, 0, 10),
			new Vector3 (0, 0, 10)
		};
		Debug.DrawLine (new Vector3 (0, 0, 0), new Vector3 (10, 0, 0), Color.red, 2000f);
		Debug.DrawLine (new Vector3 (10, 0, 0), new Vector3 (15, 0, 5), Color.red, 2000f);
		Debug.DrawLine (new Vector3 (15, 0, 5), new Vector3 (10, 0, 10), Color.red, 2000f);
		Debug.DrawLine (new Vector3 (10, 0, 10), new Vector3 (0, 0, 10), Color.red, 2000f);
		Debug.DrawLine (new Vector3 (0, 0, 10), new Vector3 (0, 0, 0), Color.red, 2000f);
		LSystem lsystem = new LSystem (new Axiom("Footprint", footprint), 10);
		List<Symbol> symbols = 
			lsystem
			.add (r1)
			.add (r2)
			.add (r3)
			.add (r4)
			.add (r5)
			.add (r6)
			.add (r7)
			.add (r8)
			.add (r9)
			.add (r10)
			.add (r11)
			.add (r12)
			.executeRules ();
		
		drawResult (symbols);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void drawResult(List<Symbol> symbols) {
		Debug.Log ("T");
		Vector3 pc = symbols [0].extraValues ["center"];
		Vector3[] normals = new Vector3[symbols.Count];
		Vector3[] centers = new Vector3[2*symbols.Count];
		int numberOfWrongs = 0;
		for (int i = 0; i < symbols.Count; i++) {
			List<Vector3> points = symbols[i].getPoints ();
			Vector3 c1 = new Vector3 (0, 0, 0);
			Vector3 c2 = new Vector3 (0, 0, 0);
			for (int j = 0; j < points.Count-1; j++) {
				c1 += points [j] / (points.Count-1);
				c2 += points [j + 1] / (points.Count - 1);
			}
			centers [2*i] = c1;
			centers [2 * i + 1] = c2;
			Vector3 n = Vector3.Cross (points [1] - points [0], points [2] - points [0]).normalized;
			normals [i] = n;
			if (Vector3.Dot (n + c1, pc) < 0) {
				numberOfWrongs++;
			}

		}
		if (numberOfWrongs > Mathf.FloorToInt((float)symbols.Count / 2f)) {
			normals = flipNormals (normals);
		}
		for (int i = 0; i < normals.Length; i++) {
			Debug.DrawRay (centers[2*i], normals[i], Color.yellow, 2000f);
		}




		Vector3[] meshPoints = new Vector3[symbols.Count*6];
		Vector2[] topAndBottomPoints = new Vector2[symbols.Count];
		for(int i = 0; i < symbols.Count; i++) {
			topAndBottomPoints [i] = new Vector2(symbols [i].getPoint (0).x, symbols[i].getPoint(0).z);
		}
		Triangulator tr = new Triangulator (topAndBottomPoints);
		int[] indices = tr.Triangulate ();
		Debug.Log ("Found " + indices.Length + " vertices");
		int[] triangles = new int[symbols.Count * 6+indices.Length*2];
		Vector3[] meshNormals = new Vector3[symbols.Count*6];
		for (int i = 0; i < symbols.Count; i++) {
			List<Vector3> symbolPoints = symbols [i].getPoints ();
			for(int j = 0; j < symbolPoints.Count; j++) {
				meshPoints [i * 4 + j] = symbolPoints [j];
				meshNormals [i * 4 + j] = normals [i];
			}
			triangles [i * 6] = i * 4+2;
			triangles [i * 6 + 1] = i * 4 + 1;
			triangles [i * 6 + 2] = i * 4 + 0;
			triangles [i * 6 + 3] = i * 4 + 2;
			triangles [i * 6 + 4] = i * 4 + 3;
			triangles [i * 6 + 5] = i * 4 + 1;
			meshPoints [symbols.Count * 4 + i] = symbolPoints [0];
			meshPoints [symbols.Count * 5 + i] = symbolPoints [2];
			meshNormals [symbols.Count * 4 + i] = Vector3.up;
			meshNormals [symbols.Count * 5 + i] = Vector3.down;
				
		}



		for (int i = 0; i < indices.Length; i++) {
			triangles [symbols.Count * 6 + +indices.Length-1-i] = indices [i] + symbols.Count * 4;
			triangles [symbols.Count * 6 + indices.Length + i] = indices [i] + symbols.Count * 5;
		}


		Mesh msh = new Mesh();
		msh.name = "Building";
		msh.vertices = meshPoints;
		msh.triangles = triangles;
		msh.normals = meshNormals;

		GameObject meshObject = new GameObject ();
		meshObject.AddComponent<MeshFilter> ().mesh = msh;
		Renderer renderer = meshObject.AddComponent<MeshRenderer> ();
		meshObject.AddComponent<TextureCellular> ();
		meshObject.transform.parent = this.transform;

		Material newMat = Resources.Load("Materials/House") as Material;
		renderer.material = newMat;
			
	}

	private Vector3[] flipNormals(Vector3[] normals) {
		Vector3[] nx = new Vector3 [normals.Length];
		for(int i = 0; i < normals.Length; i++) {
			nx [i] = normals [i] * -1;
		}
		return nx;
	}
}

