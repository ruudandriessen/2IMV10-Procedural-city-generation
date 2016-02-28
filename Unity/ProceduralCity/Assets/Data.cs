
using System;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Data : Singleton<Data>
	{
		protected Data ()
		{
		}

		public IDictionary nodes = new Dictionary<long, OsmNode>();
		public IDictionary ways = new Dictionary<long, OsmWay>();
		public bool dataLoaded = false;
	}
}

