using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class OsmEntity
	{
		public OsmEntity (long id, List<OsmTag> tags)
		{
			this.id = id;
			this.tags = tags;
		}
		private long id;
		private List<OsmTag> tags;


		public long getId() {
			return this.id;
		}

		public int getNumberOfTags() {
			return tags.Count;
		}

		public OsmTag getTag(int n) {
			return tags [n];
		}
	}
}

