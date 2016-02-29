using UnityEngine;
using System.Collections;
using ProceduralCity;

public class Initialize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadData.Load ();
		Map.loadOrigin ();
		GenerateRoads.Generate ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
