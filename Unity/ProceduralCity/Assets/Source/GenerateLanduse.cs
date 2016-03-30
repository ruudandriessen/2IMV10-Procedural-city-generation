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
			GameObject.Destroy (terrain.GetComponent<BoxCollider> ());
			terrain.isStatic = true;
			terrain.transform.localScale = max;

			terrain.GetComponent<Renderer> ().material = Resources.Load("Materials/ground2", typeof(Material)) as Material;

			// Get surfaces
			foreach (long key in Data.Instance.surfaces.Keys) {
				GameObject obj = new GameObject("Surface");
				Surface script = obj.AddComponent<Surface> ();
				script.surfaceId = key;
			}
		}
	}
}

