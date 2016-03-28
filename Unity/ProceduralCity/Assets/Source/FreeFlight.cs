using UnityEngine;
using System.Collections;

public class FreeFlight : MonoBehaviour
{
	public float flySpeed = 0.5f;
	public GameObject defaultCam;
	public GameObject playerObject;

	void Update()
	{
		if (Input.GetKey (KeyCode.LeftShift)) {
			flySpeed = 50f;
		} else {
			flySpeed = 0.5f;
		}

		if(Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
		
		if (Input.GetKey(KeyCode.W))
		{
			transform.position = transform.position + (defaultCam.transform.forward * flySpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			transform.position = transform.position + (-defaultCam.transform.forward * flySpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.position = transform.position + (-defaultCam.transform.right * flySpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.position = transform.position + (defaultCam.transform.right * flySpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.M))
			playerObject.transform.position = transform.position; //Moves the player to the flycam's position. Make sure not to just move the player's camera.
	}
}