using System;
using UnityEngine;

namespace ProceduralCity
{
	public class GenerateLanduse
	{
		public static void Generate () {
			// Get buildings
			foreach (long key in Data.Instance.surfaces.Keys) {
				GameObject obj = new GameObject("Surface");
				Surface script = obj.AddComponent<Surface> ();
				script.surfaceId = key;
			}
		}
	}
}

