using UnityEngine;
using System.Collections;
using ProceduralCity;

public class BuildingGenerationScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		GameObject g = new GameObject ("subscope");
		g.transform.parent = this.transform;
		Rule r = new Rule ()
			.add (new InsertOperation (g, PrimitiveType.Cube))
			.add (new ScaleOperation (g, 1, 5, 1))
			.add (new TranslateOperation (g, 0, 0, 6))
			.add (new RotateOperation(g, 0, 10, 0))
			.add (new InsertOperation (gameObject, PrimitiveType.Cylinder))
			.add (new ScaleOperation (gameObject, 2,1, 1));

		LSystem lsystem = new LSystem ();
		lsystem.add (r)
			.executeRules ();


	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

