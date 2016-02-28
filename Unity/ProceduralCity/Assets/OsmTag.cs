using System;

namespace AssemblyCSharp
{
	public class OsmTag
	{
		public OsmTag (string key, string value)
		{
			this.key = key;
			this.value = value;
		}

		private string key;
		private string value;

		public string getKey() {
			return this.key;
		}

		public string getValue() {
			return this.value;
		}
	}
}

