using System;

namespace ProceduralCity
{
	public class Condition
	{
		private Object compareValue;
		private Object value; 
		private string operation;

		public Condition ()
		{
		}

		public void addCompareValue(Object compareValue) {
			this.compareValue = compareValue;
		}

		public void addValue(Object value) {
			this.value = value;
		}

		public void addOperation(string operation) {
			this.operation = operation;
		}

		public bool evaluateCondition() {
			if(compareValue.GetType() != value.GetType()) {
				throw new ArgumentException ("Arguments not of same type");
			}
			if (operation == "==") {
				return value == compareValue;
			}
			if (operation == ">") {
				return (int) value > (int) compareValue;
			}
			return false;
		}
	}
}

