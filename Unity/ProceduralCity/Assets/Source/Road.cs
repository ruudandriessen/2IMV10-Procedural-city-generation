using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Road : MonoBehaviour {

	public List<Vector3> points;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < this.points.Count - 1; i++) {
			GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

//			GameObject c1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			GameObject c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			c1.transform.position = points [i];
//			c2.transform.position = points [i+1];
			PositionSize(points [i], points[i+1], plane);
			plane.transform.parent = this.transform.parent;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void PositionSize(Vector3 start, Vector3 end, GameObject g) {
		Vector3 pos = (end - start) / 2.0f + start;  // Position
		g.transform.position = pos;

		Vector3 v3 = end-start;    // Rotation
		g.transform.rotation = Quaternion.FromToRotation(Vector3.right, v3);
		g.transform.localScale = new Vector3(v3.magnitude/10.0f, 0.01f, 0.01f);  // Scale 
	}
}
