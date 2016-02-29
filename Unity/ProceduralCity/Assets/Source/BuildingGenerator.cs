using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LibTessDotNet;

namespace ProceduralCity
{
	public class BuildingGenerator
	{
		public BuildingGenerator ()
		{
		}

		// The data array contains 4 values, it's the associated data of the vertices that resulted in an intersection.
		private static object VertexCombine(LibTessDotNet.Vec3 position, object[] data, float[] weights)
		{
			/*// Fetch the vertex data.
			var colors = new Color[] { (Color)data[0], (Color)data[1], (Color)data[2], (Color)data[3] };
			// Interpolate with the 4 weights.
			var rgba = new float[] {
				(float)colors[0].r * weights[0] + (float)colors[1].r * weights[1] + (float)colors[2].r * weights[2] + (float)colors[3].r * weights[3],
				(float)colors[0].g * weights[0] + (float)colors[1].g * weights[1] + (float)colors[2].g * weights[2] + (float)colors[3].g * weights[3],
				(float)colors[0].b * weights[0] + (float)colors[1].b * weights[1] + (float)colors[2].b * weights[2] + (float)colors[3].b * weights[3],
				(float)colors[0].a * weights[0] + (float)colors[1].a * weights[1] + (float)colors[2].a * weights[2] + (float)colors[3].a * weights[3]
			};
			// Return interpolated data for the new vertex.
			//return Color.FromArgb((int)rgba[3], (int)rgba[0], (int)rgba[1], (int)rgba[2]);
			Debug.Log(new Color (rgba [0], rgba [1], rgba [2], rgba [3]).ToString());*/
			//return new Color (rgba [0], rgba [1], rgba [2], rgba [3]);
			return new Color(0f, 1f, 1f, 1f);
		}


		public void generateBuildings() {
			/*Debug.Log ("Starting to do stuff");
			int x = 0;
			// Get roads
			foreach (long key in Data.Instance.buildings.Keys) {
				OsmBuilding building = Data.Instance.buildings [key];
				//float[] points = building.getPolygon ();
				Vector2[] points = building.getPolygonAsVector2 ();
				var tess = new LibTessDotNet.Tess ();
				var contour = new LibTessDotNet.ContourVertex[points.Length];
				for (int i = 0; i < points.Length/2; i++)
				{
					// NOTE : Z is here for convenience if you want to keep a 3D vertex position throughout the tessellation process but only X and Y are important.
					contour[i].Position = new LibTessDotNet.Vec3 { X = points[i * 2], Y = points[i * 2 + 1], Z = 0.0f };
					// Data can contain any per-vertex data, here a constant color.
					contour[i].Data = Color.cyan;
				}
				tess.AddContour(contour, LibTessDotNet.ContourOrientation.Clockwise);
				tess.Tessellate(LibTessDotNet.WindingRule.EvenOdd, LibTessDotNet.ElementType.Polygons, 3, VertexCombine);
				int numTriangles = tess.ElementCount;

*/


				//Debug.Log (numTriangles);

				/*List<Vector3> buildings = new List<Vector3>();
			OsmWay way = (OsmWay) Data.Instance.ways[key];
			for (int i = 0; i < way.getNumberOfNodes (); i++) {
				OsmNodeReference node = way.getNodeReference(i);
				road.Add(new Vector3(node.getLattitude () * 10000.0f, 0.0f, node.getLongitude() * 10000.0f));
			}
			createRoad(road);*/
	

			//Debug.Log(x + " buildings done");
		}


	}
}

