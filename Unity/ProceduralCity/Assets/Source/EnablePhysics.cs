using System;
using UnityEngine;

namespace ProceduralCity
{
	public class EnablePhysics : MonoBehaviour
	{
		public EnablePhysics ()
		{
		}

		void Update() {
			if (Input.GetKeyDown("space")){
				GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
				foreach(GameObject go in allObjects) {
					Rigidbody body = go.GetComponent<Rigidbody>();
					body.isKinematic = false;
				}
			}
		}
	}
}

