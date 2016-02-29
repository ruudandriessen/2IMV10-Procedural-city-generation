using System;

namespace ProceduralCity
{
	public class OsmRelationMember
	{
		public OsmRelationMember (long id, EntityType entityType, string role)
		{
			this.id = id;
			this.entityType = entityType;
			this.role = role;
		}

		private long id;
		private EntityType entityType;
		private string role;

		public long getId() {
			return this.id;
		}

		public EntityType getEntityType() {
			return this.entityType;
		}

		public string getRole() {
			return this.role;
		}
	}
}

