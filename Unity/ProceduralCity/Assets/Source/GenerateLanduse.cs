using System;
using UnityEngine;

namespace ProceduralCity
{
	public class GenerateLanduse
	{
		public static void Generate () {
			Vector3 min = Map.getMin ();
			Vector3 max = Map.getMax ();
			min.y -= 0.15f;
			max.y -= 0.15f;

			GameObject terrain = GameObject.CreatePrimitive (PrimitiveType.Cube);
			terrain.transform.position = min + max/2;
			terrain.transform.localScale = max;
			float r = 242.0f / 256.0f;
			float g = 239.0f / 256.0f;
			float b = 233.0f / 256.0f;
						
			terrain.GetComponent<Renderer> ().material.color = new Color (r, g, b);

//			// Get surfaces
//			foreach (long key in Data.Instance.surfaces.Keys) {
//				GameObject obj = new GameObject("Surface");
//				Surface script = obj.AddComponent<Surface> ();
//				script.surfaceId = key;
//			}
		}
	}
}

