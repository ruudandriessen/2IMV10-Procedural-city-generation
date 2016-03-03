using System;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class OsmEntity
	{
		public OsmEntity (long id, List<OsmTag> tags)
		{
			this.id = id;
			this.tags = tags;
			this.relations = new List<OsmRelation> ();
		}
		private long id;
		private List<OsmTag> tags;
		private List<OsmRelation> relations;

		public long getId() {
			return this.id;
		}

		public void addRelation(OsmRelation rel) {
			relations.Add(rel);
		}

		public OsmRelation getRelation(int i) {
			return relations [i];
		}

		public List<OsmRelation> getRelations() {
			return relations;
		}

		public int getNumberOfRelations() {
			return relations.Count;
		}

		public int getNumberOfTags() {
			return tags.Count;
		}

		public OsmTag getTag(int n) {
			return tags [n];
		}
	}
}

