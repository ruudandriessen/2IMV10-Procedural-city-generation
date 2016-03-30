using UnityEngine;
using System.Collections;

public class SunMovement : MonoBehaviour {

	bool lighton = false;

	// Use this for initialization
	void Start () {
		Light[] allObjects = UnityEngine.Object.FindObjectsOfType<Light>() ;
		foreach(Light light in allObjects) {
			light.enabled = false;
		}
		GetComponent<Light> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Q)) {
			transform.Rotate (0.5f, 0.0f, 0.0f);
		} else if (Input.GetKey (KeyCode.E)) {
			transform.Rotate (-0.5f, 0.0f, 0.0f);
		} else if (Input.GetKeyDown (KeyCode.R)) {
			Light[] allObjects = UnityEngine.Object.FindObjectsOfType<Light>() ;
			foreach(Light light in allObjects) {
				light.enabled = !lighton;
			}
			GetComponent<Light> ().enabled = true;
			lighton = !lighton;
		}
	}
}
