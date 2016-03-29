using UnityEngine;
using System.Collections;

public class SunMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Q)) {
			transform.Rotate (0.5f, 0.5f, 0.5f);
		} else if (Input.GetKey (KeyCode.E)) {
			transform.Rotate (-0.5f, -0.5f, -0.5f);
		}
	}
}
