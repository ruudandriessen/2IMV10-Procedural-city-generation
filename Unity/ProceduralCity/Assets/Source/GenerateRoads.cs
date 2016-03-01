using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;

public class GenerateRoads : MonoBehaviour {
	// Use this for initialization
	public static void Generate () {
		Debug.Log ("Generating roads..");
		foreach (long key in Data.Instance.streets.Keys) {
			List<Vector3> road = new List<Vector3>();
			OsmStreet streets = (OsmStreet) Data.Instance.streets[key];
			for (int i = 0; i < streets.getNumberOfNodes (); i++) {
				OsmNodeReference node = streets.getNodeReference(i);

				float lat = node.getLattitude();
				float lon = node.getLongitude();

				Vector3 result = Map.getVectorFromOrigin (lat, lon);
				result.y += 0.01f;
				road.Add(result);
			}
			createRoad(road);
		}
		Debug.Log ("Generated " + Data.Instance.streets.Keys.Count + " roads!");
	}

	static void createRoad(List<Vector3> points) {
		GameObject obj = new GameObject("Road");
		Road script = obj.AddComponent<Road>();
		script.points = points;
	}
}
