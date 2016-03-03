using System;
using UnityEngine;

namespace ProceduralCity
{
	public class GenerateLanduse
	{
		public static void Generate () {
			Vector3 min = Map.getMin ();
			Vector3 max = Map.getMax ();
			min.y -= 0.5f;
			max.y -= 0.5f;

			GameObject terrain = GameObject.CreatePrimitive (PrimitiveType.Cube);
			terrain.transform.position = min + max/2;
			terrain.transform.localScale = max;
			terrain.GetComponent<Renderer> ().material.color = new Color (228.0f/256.0f, 220.0f/256.0f, 213.0f/256.0f);

			// Get surfaces
			foreach (long key in Data.Instance.surfaces.Keys) {
				GameObject obj = new GameObject("Surface");
				Surface script = obj.AddComponent<Surface> ();
				script.surfaceId = key;
			}
		}
	}
}

