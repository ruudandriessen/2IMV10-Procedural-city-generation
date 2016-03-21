using UnityEngine;
using System.Collections;
using ProceduralCity;

public class HouseGeneration : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Scope s = new Scope (this.gameObject);
		Rule r1 = new Rule ("Footprint")
			.add (new ScaleOperation (s, 1, 25, 1))
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
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

