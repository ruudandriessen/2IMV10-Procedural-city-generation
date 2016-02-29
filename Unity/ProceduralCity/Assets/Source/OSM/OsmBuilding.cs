using System;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class OsmBuilding : OsmWay
	{
		public OsmBuilding (long id, List<OsmTag> tags, List<OsmNodeReference> nodes) : base (id, tags, nodes)
		{
		}
	}
}

