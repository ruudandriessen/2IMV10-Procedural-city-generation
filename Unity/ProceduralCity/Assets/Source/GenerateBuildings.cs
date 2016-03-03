using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;
using LibTessDotNet;

public class GenerateBuildings : MonoBehaviour {
	public static void Generate () {
		// Get buildings
		foreach (long key in Data.Instance.buildings.Keys) {
			GameObject obj = new GameObject("Building");
			Building script = obj.AddComponent<Building> ();
			script.buildingId = key;
		}
	}
}
