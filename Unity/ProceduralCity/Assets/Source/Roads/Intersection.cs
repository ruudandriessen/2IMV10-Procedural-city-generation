using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intersection : MonoBehaviour {

	public List<Vector3> roadDirections = new List<Vector3>();
	public Vector3 position;

	// Use this for initialization
	void Start () {
		GameObject intersection = GameObject.CreatePrimitive (PrimitiveType.Cube);
		intersection.transform.parent = this.transform;
		intersection.transform.position = position;

//		foreach (Vector3 dir in roadDirections) {
//			CreateVertex (position, dir, -1.0f);
//			CreateVertex (position, dir, 1.0f);
//		}
	}

	public void addDirection(Vector3 dir) {
		roadDirections.Add (dir);
		Debug.DrawRay (position, dir, Color.cyan, 200);
	}

//	Vector3 CreateVertex(Vector3 pos, Vector3 dir,  float offsetX) {
//		Vector3 d1 = pos + dir; 
//
//		Vector3 result = (d1.normalized + d2.normalized).normalized * offsetX;
//		Vector3 cross = Vector3.Cross (d1, d2).normalized;
//
//		if (cross.y > 0) {
//			return pivot - result;
//		} else {
//			return pivot + result;
//		}
//	}

	// Update is called once per frame
	void Update () {
	}
}