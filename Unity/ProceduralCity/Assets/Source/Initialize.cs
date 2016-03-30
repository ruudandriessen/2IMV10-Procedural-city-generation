using UnityEngine;
using System.Collections;
using ProceduralCity;

public class Initialize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadData.Load ();
		Map.loadOrigin ();
		GenerateRoads.Generate ();
		GenerateBuildings.Generate ();
		GenerateLanduse.Generate ();
		foreach (long key in Data.Instance.buildings.Keys) {
			GameObject.Find ("Main Camera").transform.position = Data.Instance.buildings [key].getPolygonAsVector3 () [0];
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
