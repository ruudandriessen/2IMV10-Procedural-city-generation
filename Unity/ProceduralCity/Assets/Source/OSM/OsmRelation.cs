using System;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class OsmRelation : OsmEntity
	{
		public OsmRelation (long id, List<OsmTag> tags, List<OsmRelationMember> members) : base(id, tags) {
			this.members = members;
		}
		private List<OsmRelationMember> members;

		public int getNumberOfMembers() {
			return members.Count;
		}

		public OsmRelationMember getMember(int n) {
			return members [n];
		}
	}
}

