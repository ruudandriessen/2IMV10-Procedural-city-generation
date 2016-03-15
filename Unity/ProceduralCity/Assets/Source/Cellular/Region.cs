using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ProceduralCity
{
	public class Region
	{
		private List<Vertex> plane;

		public Region (List<Vertex> points)
		{
			plane = points;
		}

		public void addPlanePoint(Vertex p){
			plane.Add (p);
		}

		public List<Vertex> getPoints() {
			return plane;
		}
	}
}

