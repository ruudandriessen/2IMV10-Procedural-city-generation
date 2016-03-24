using System;
using UnityEngine;

namespace ProceduralCity
{
	public class FillCellModule
	{
		public FillCellModule ()
		{
		}

		public static void fillCell(Vector3 start, Vector3 end, Vector3 size, Vector3 dimensions, Transform parent) {
			// Calculate edge and it's magnitude
			Vector3 edge = end-start;
			Vector3 direction = edge.normalized;
			float magnitude = edge.magnitude;

			Quaternion rotation = Quaternion.FromToRotation (direction, Vector3.forward); // Rotation based off the z (forward) axis
			int numCells = (int) Mathf.Floor(magnitude / dimensions.z) + 1; // Floor the number of cells and add one (no Roof method is available)
			float magnitudeOvershoot = -(magnitude - numCells * dimensions.z);
			Vector3 startCenter = start + direction * dimensions.z / 2;

			float overshootSize = dimensions.z - magnitudeOvershoot;
//			Debug.Log ("---");
//			Debug.Log("Dimensions: " + dimensions.ToString("F4"));
//			Debug.Log("Size: " + size.ToString("F4"));
//			Debug.Log (magnitude + " - " + numCells + " * " + dimensions.z);
//			Debug.Log ("magnOvershoot: " + magnitudeOvershoot);
//			Debug.Log ("Final size: " + overshootSize);
			for (int i = 0; i < numCells; i++) {
				Vector3 p = startCenter + i * direction * dimensions.z;
				if (i == numCells - 1) {
					if (overshootSize > 0.01f) {
						size.z = overshootSize;
						p -= magnitudeOvershoot / 2 * direction;
						Cell c = new Cell (parent, p, size, rotation, "CellOvershoot");
					}
				} else {
					Cell c = new Cell (parent, p, size, rotation, "Cell");	
				}
			}
		}
	}
}

