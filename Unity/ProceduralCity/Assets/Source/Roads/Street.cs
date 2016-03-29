using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProceduralCity;

public class Street : MonoBehaviour {

	public long streetId;
	OsmStreet streetData;

	// Use this for initialization
	void Start () {
		initializeData ();
		splitPointsInRoads ();
	}

	private void initializeData() {	
		this.streetData = Data.Instance.streets [streetId];
	}

	private void splitPointsInRoads() {
		List<Vector3> dataPoints = new List<Vector3> ();
		int size = streetData.getNumberOfNodes();
		for (int i = 0; i < size; i++) {
			OsmNode n = streetData.getNode(i);
			Vector3 p = Map.getVectorFromOrigin 
				(n.getLatitude(), n.getLongitude());
			p.y = 0.1f;
			dataPoints.Add (p);
			if (isIntersection (n)) {
				// Create intersection
				Vector3 p1 = Vector3.zero;
				Vector3 p2 = Vector3.zero;
				if (i > 0) {
					OsmNode n1 = streetData.getNode(i - 1);
					p1 = Map.getVectorFromOrigin 
						(n1.getLatitude(), n1.getLongitude());
				}
				if (i < size - 1) {
					OsmNode n2 = streetData.getNode(i + 1);
					p2 = Map.getVectorFromOrigin 
						(n2.getLatitude(), n2.getLongitude());
				}

				createIntersection (p, p1, p2);

				// Create road from points so far, if they are at least 2 in size
				if (dataPoints.Count > 1) {
					createRoad (dataPoints.ToArray ());
					dataPoints.Clear ();
					dataPoints.Add (p);
				}
			}
		}
		if (dataPoints.Count > 1) 
			createRoad (dataPoints.ToArray ());
	}

	private void createIntersection(Vector3 node, Vector3 dir1, Vector3 dir2) {
		GameObject intersectionObject = new GameObject ("Intersection");
		Intersection intersection = intersectionObject.AddComponent<Intersection> ();
		intersectionObject.name = "Intersection";
		intersection.transform.parent = this.transform;

		intersection.position = node;
		if (dir1 != Vector3.zero) {
			intersection.addDirection (-1 * (node - dir1).normalized);
		}
		if (dir2 != Vector3.zero) {
			intersection.addDirection (-1 * (node - dir2).normalized);
		}
	}

	private void createRoad(Vector3[] dataPoints) {
		Road road = this.gameObject.AddComponent<Road> ();
		road.points = dataPoints;
	}

	private bool isIntersection(OsmNode node) {
		return node.amountContainedIn () > 1;
	}

	// Update is called once per frame
	void Update () {
	}
}
