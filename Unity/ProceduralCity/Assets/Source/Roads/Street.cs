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
			p.y = 0.05f;
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

		for (int i = 0; i < dataPoints.Length - 1; i++) {
			Vector3 p1 = dataPoints [i];
			Vector3 p2 = dataPoints [i + 1];

			// Create lantern
			GameObject lantern = new GameObject ();
			lantern.name = "Lantern";
			Vector3 transDirection = Vector3.Cross (p1 - p2, Vector3.up).normalized;
			lantern.transform.position = (p1 + p2) / 2 + transDirection * 1.2f;
			lantern.transform.position += new Vector3(0, 0.5f, 0);

			// Create the lightpost
			GameObject lightPost = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			lightPost.transform.parent = lantern.transform;
			lightPost.transform.position = lantern.transform.position;
			lightPost.transform.localScale = new Vector3 (0.2f, 3.0f, 0.2f);
			Destroy (lightPost.GetComponent<CapsuleCollider> ());

			// Create the latern top towards the road
			GameObject lightHandle = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			lightHandle.transform.parent = lantern.transform;
			lightHandle.transform.position = lantern.transform.position + new Vector3 (0, 3.2f, 0);
			Destroy (lightHandle.GetComponent<SphereCollider> ());
			lightHandle.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
			lightHandle.GetComponent<MeshRenderer> ().material = Resources.Load("Materials/Glass 1", typeof(Material)) as Material;
//			lightHandle.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			// Create the light component
			GameObject light = new GameObject ("Light");
			Light lightComp = light.AddComponent<Light>();
			lightComp.type = LightType.Point;
			lightComp.spotAngle = 20.0f;
			lightComp.color = Color.white;
			lightComp.renderMode = LightRenderMode.ForcePixel;
			lightComp.intensity = 10.0f;
			lightComp.shadowStrength = 0.5f;
			lightComp.shadows = LightShadows.Hard;
			lightComp.transform.parent = lantern.transform;
			lightComp.transform.position = lantern.transform.position + new Vector3 (0, 3.2f, 0);

		}
		road.points = dataPoints;
	}

	private bool isIntersection(OsmNode node) {
		return node.amountContainedIn () > 1;
	}

	// Update is called once per frame
	void Update () {
	}
}
