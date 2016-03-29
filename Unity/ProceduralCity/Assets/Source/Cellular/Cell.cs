using System;
using UnityEngine;

namespace ProceduralCity
{
	public class Cell
	{
		GameObject cell;

		public Cell (Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, String name) : 
		this(parent, position, scale, rotation, name, Vector3.zero)
		{ 
			// Only calling other constructor	
		}

		public Cell (Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, String name, Vector3 normal)
		{
			cell = GameObject.CreatePrimitive(PrimitiveType.Cube);

			cell.GetComponent<BoxCollider> ().enabled = false;
			cell.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			// Get renderer and apply material
//			MeshRenderer renderer = cell.GetComponent<MeshRenderer>();
//			MeshFilter filter = cell.GetComponent<MeshFilter>();
//			Material newMat = Resources.Load("Materials/Concrete_Asphalt_02", typeof(Material)) as Material;
//			renderer.sharedMaterial = newMat;
//			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

//			if (normal != Vector3.zero) {
//				int count = filter.mesh.normals.Length;
//				for (int i = 0; i < count; i++) {
//					filter.mesh.normals [i] = normal;
//					
//				}
//			}
//
//			float x = scale.x;
//			scale.x = scale.z;
//			scale.z = x;

			// Set parameters
			cell.name = name;
			cell.transform.localScale = scale;
			cell.transform.localRotation = rotation;// * Quaternion.AngleAxis(90, Vector3.up);
			cell.transform.localPosition = position;

			// Improve performance by marking as static
			cell.isStatic = true;
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

		public GameObject getCell() {
			return cell;
		}
	}
}

