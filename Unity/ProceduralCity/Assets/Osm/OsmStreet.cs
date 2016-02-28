using System;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class OsmStreet : OsmWay
	{
		public OsmStreet (long id, List<OsmTag> tags, List<OsmNodeReference> nodes) : base (id, tags, nodes)
		{
		}
	}
}

