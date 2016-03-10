using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ProceduralCity
{
	public class Region
	{
		private List<Vector3> plane;

		public Region (List<Vector3> points)
		{
			plane = points;
		}

		public void addPlanePoint(Vector3 p){
			plane.Add (p);
		}
	}
}

