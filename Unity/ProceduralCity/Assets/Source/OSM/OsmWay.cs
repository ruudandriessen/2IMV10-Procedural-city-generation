using System;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class OsmWay : OsmEntity
	{
		public OsmWay (long id, List<OsmTag> tags, List<OsmNodeReference> nodes) : base(id, tags)
		{
			this.nodes = nodes;
		}

		private List<OsmNodeReference> nodes;

		public int getNumberOfNodes() {
			return nodes.Count;
		}

		public long getNodeId(int n) {
			return nodes [n].getId();
		}

		public OsmNodeReference getNodeReference(int n) {
			return nodes [n];
		}

		public OsmNode getNode(int n) {
			return Data.Instance.nodes[nodes [n].getId ()];
		}
			
	}
}

