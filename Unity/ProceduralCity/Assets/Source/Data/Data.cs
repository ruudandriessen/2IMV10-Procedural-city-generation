
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class Data : Singleton<Data>
	{
		protected Data ()
		{
		}

		public Dictionary<long, OsmNode> nodes = new Dictionary<long, OsmNode>();
		public Dictionary<long, OsmWay> ways = new Dictionary<long, OsmWay>();
		public Dictionary<long, OsmRelation> relations = new Dictionary<long, OsmRelation> ();

		public Dictionary<long, OsmBuilding> buildings = new Dictionary<long, OsmBuilding> ();
		public Dictionary<long, OsmStreet> streets = new Dictionary<long, OsmStreet> ();
	
		public bool dataLoaded = false;
	}
}

