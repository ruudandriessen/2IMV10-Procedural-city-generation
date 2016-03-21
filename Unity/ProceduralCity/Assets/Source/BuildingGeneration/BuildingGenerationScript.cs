using UnityEngine;
using System.Collections;
using ProceduralCity;

public class BuildingGenerationScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		GameObject g1 = new GameObject ("Scope 1");
		g1.transform.parent = this.transform;
		Scope s = new Scope (this.gameObject);
		Scope s1 = new Scope (g1);
		Rule r = new Rule ("A")
			.add (new TranslateOperation (s1, 0, 0, 6))
			.add (new ScaleOperation (s1, 8, 10, 18))
			.add (new InsertOperation (s1, PrimitiveType.Cube))
			.add (new TranslateOperation (s, 6, 0, 0))
			.add (new ScaleOperation (s, 7, 13, 18))
			.add (new InsertOperation (s, PrimitiveType.Cube))
			.add (new TranslateOperation (s, 0, 0, 16))
			.add (new ScaleOperation (s, 8, 15f/2f, 8))
			.add (new InsertOperation (s, PrimitiveType.Cylinder));

			//.add (new TranslateOperation (g3, 8 / 2, 15 / 2, 8 / 2))
			//.add (new TranslateOperation (g3, 0, 0, 16));

		LSystem lsystem = new LSystem (new Axiom("A", null), 1);
		lsystem.add (r)
			.executeRules ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

