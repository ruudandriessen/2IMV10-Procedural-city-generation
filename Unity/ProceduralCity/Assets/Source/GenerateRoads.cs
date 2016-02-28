using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GenerateRoads : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Get roads
		foreach (long key in Data.Instance.ways.Keys) {
			List<Vector3> road = new List<Vector3>();
			OsmWay way = (OsmWay) Data.Instance.ways[key];
			for (int i = 0; i < way.getNumberOfNodes (); i++) {
				OsmNodeReference node = way.getNodeReference(i);
				road.Add(new Vector3(node.getLattitude () * 10000.0f, 0.0f, node.getLongitude() * 10000.0f));
			}
			createRoad(road);
		}
		// for
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void createRoad(List<Vector3> points) {
		GameObject obj = new GameObject("Road");
		Road script = obj.AddComponent<Road>();
		script.points = points;
	}
}
