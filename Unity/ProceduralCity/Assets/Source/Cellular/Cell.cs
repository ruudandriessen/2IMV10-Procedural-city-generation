using System;
using UnityEngine;

namespace ProceduralCity
{
	public class Cell
	{
		GameObject cell;

		public Cell (Transform parent, Vector3 position, Vector3 scale, Quaternion rotation)
		{ 
			cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cell.transform.parent = parent;
			cell.transform.localRotation = rotation;
			cell.transform.localScale = scale;
			cell.transform.position = position;
		}

		public void setColor(Color c) {
			cell.GetComponent<Renderer> ().material.color = c;
		}
	}
}

