using System;

namespace AssemblyCSharp
{
	public enum EntityType
	{
		Node,
		Way,
		Relation,

	}

	static class EntityTypeMethods
	{
		public static EntityType fromString(string x) {
			if (x == "way") {
				return EntityType.Way;
			} else if (x == "relation") {
				return EntityType.Relation;
			} else {
				return EntityType.Node;
			}
		}
	}
		
}

