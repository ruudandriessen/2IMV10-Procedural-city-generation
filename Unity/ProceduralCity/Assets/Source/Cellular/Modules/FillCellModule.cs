using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class FillCellModule
	{
		public FillCellModule ()
		{
		}

		public static List<MeshFilter> fillCell(Vector3 start, Vector3 end, Vector3 size, Vector3 dimensions, Transform parent, Color color, Vector3 normal) {
			List<MeshFilter> meshes = new List<MeshFilter> ();

			// Calculate edge and it's magnitude
			Vector3 edge = end-start;
			Vector3 direction = edge.normalized;
			float magnitude = edge.magnitude;

			Quaternion rotation = Quaternion.FromToRotation (Vector3.forward, direction);

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
						Cell c = new Cell (parent, p, size, rotation, "CellOvershoot", normal);
						c.setColor(color);
						meshes.Add(c.getCell ().GetComponent<MeshFilter> ());
					}
				} else {
					Cell c = new Cell (parent, p, size, rotation, "Cell", normal);
					c.setColor(color);
					meshes.Add (c.getCell ().GetComponent<MeshFilter> ());
				}
			}
			return meshes;
		}
	}
}

