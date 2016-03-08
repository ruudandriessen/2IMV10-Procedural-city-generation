using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;
using System;

public class GenerateRoads : MonoBehaviour {
	// Use this for initialization
	public static void Generate () {
		Debug.Log ("Generating streets..");
		foreach (long key in Data.Instance.streets.Keys) {
			createStreet(key);
		}
		Debug.Log ("Generated " + Data.Instance.streets.Keys.Count + " streets!");
	}

	static OsmEntity getEntity(OsmRelationMember m) {
		OsmEntity entity = null;
		try { 
			switch (m.getEntityType()) {
			case EntityType.Node: 
				entity = Data.Instance.nodes [m.getId ()];
			break;
			case EntityType.Way: 
				entity = Data.Instance.ways [m.getId ()];
			break;
			case EntityType.Relation: 
				entity = Data.Instance.relations [m.getId ()];
			break;
			}
		} catch (Exception e) {
			Debug.Log ("Warning - Not existing relation node " + m.getId() + " : " + m.getEntityType());
		}
		return entity;
	}

	static void createStreet(long id) {
		GameObject obj = new GameObject("Street");
		Street script = obj.AddComponent<Street>();
		script.streetId = id;
	}
}
