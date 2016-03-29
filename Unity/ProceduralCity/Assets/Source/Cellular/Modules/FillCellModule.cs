using System;
using UnityEngine;

namespace ProceduralCity
{
	public class FillCellModule
	{
		public FillCellModule ()
		{
		}

		public static void fillCell(Vector3 start, Vector3 end, Vector3 size, Vector3 dimensions, Transform parent, Color color, Quaternion rotation) {
			// Calculate edge and it's magnitude
			Vector3 edge = end-start;
			Vector3 direction = edge.normalized;
			float magnitude = edge.magnitude;

			int numCells = (int) Mathf.Floor(magnitude / dimensions.z) + 1; // Floor the number of cells and add one (no Roof method is available)
			float magnitudeOvershoot = -(magnitude - numCells * dimensions.z);
			Vector3 startCenter = start + direction * dimensions.z / 2;

			float overshootSize = dimensions.z - magnitudeOvershoot;

			for (int i = 0; i < numCells; i++) {
				Vector3 p = startCenter + i * direction * dimensions.z;
				if (i == numCells - 1) {
					if (overshootSize > 0.01f) {
						size.z = overshootSize;
						p -= magnitudeOvershoot / 2 * direction;
						Cell c = new Cell (parent, p, size, rotation, "CellOvershoot");
						c.setColor(color);
					}
				} else {
					Cell c = new Cell (parent, p, size, rotation, "Cell");
					c.setColor(color);
				}
			}
		}

		public static void fillCell(Vector3 start, Vector3 end, Vector3 size, Vector3 dimensions, Transform parent, Color color) {
			// Calculate edge and it's magnitude
			Vector3 edge = end-start;
			Vector3 direction = edge.normalized;

			Quaternion rotation = Quaternion.FromToRotation (Vector3.forward, direction); // Rotation based off the z (forward) axis
			fillCell(start, end, size, dimensions, parent, color, rotation);
		}
	}
}

