using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateRoads : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Get roads
		List<Vector3> r1 = new List<Vector3>();
		r1.Add(new Vector3(10, 0, 0));
		r1.Add(new Vector3(5, 0, 5));
		r1.Add(new Vector3(0, 0, 10));

		// for
		createRoad(r1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void createRoad(List<Vector3> points) {
		GameObject obj = new GameObject();
		Road script = obj.AddComponent<Road>();
		script.points = points;
	}
}
