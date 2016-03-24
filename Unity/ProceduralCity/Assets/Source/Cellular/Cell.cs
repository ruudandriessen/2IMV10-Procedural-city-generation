using System;
using UnityEngine;

namespace ProceduralCity
{
	public class Cell
	{
		GameObject cell;

		public Cell (Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, String name)
		{ 
			cell = GameObject.CreatePrimitive(PrimitiveType.Cube);

			// Add rigidbody and disable movement by default
			Rigidbody body = cell.AddComponent<Rigidbody> ();
			body.useGravity = true;
			body.isKinematic = true;

			// Get renderer and apply material
			MeshRenderer renderer = cell.GetComponent<MeshRenderer>();
			Material newMat = Resources.Load("Materials/Concrete_Asphalt_02", typeof(Material)) as Material;
			renderer.material = newMat;

			cell.name = name;
//			cell.transform.parent = parent; //This seems to break everything when using non-uniform scaled parents with rotations derp.

			cell.transform.localRotation = rotation;
//			scale = div (scale, parent.lossyScale);
			cell.transform.localScale = scale;

			cell.transform.localPosition = position;
		}

		public void setColor(Color c) {
			cell.GetComponent<Renderer> ().material.color = c;
		}

		static public Vector3 div(Vector3 v1, Vector3 v2) {
			v1.x /= v2.x;
			v1.y /= v2.y;
			v1.z /= v2.z;
			return v1;
		}
	}
}

