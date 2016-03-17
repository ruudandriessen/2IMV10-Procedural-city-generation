using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class Rule
	{
		List<Operation> operations;
		//Predecessor is the name of the symbols that are relevant for this rule
		private string predeccessor;

		public Rule (string predecessor)
		{
			this.predeccessor = predecessor;
			operations = new List<Operation> ();
		}
		private Condition condition;
		private double probability= 1;

		public Condition getCondition() {
			return this.condition;
		}

		public double getProbability() {
			return this.probability;
		}

		public Symbol getSuccessor(Symbol predecessor) {
			return predecessor;
		}

		public Rule add(Operation o) {
			operations.Add(o);
			return this;
		}

		public void execute(Symbol s) {
			for (int i = 0; i < operations.Count; i++) {
				operations [i].applyOperation ();
			}
		}

		public string getPredeccessorType() {
			return this.predeccessor;
		}





	}
}

