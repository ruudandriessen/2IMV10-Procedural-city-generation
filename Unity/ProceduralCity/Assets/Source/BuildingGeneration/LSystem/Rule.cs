using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class Rule
	{
		List<Operation> operations;

		public Rule ()
		{
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

		public void execute() {
			for (int i = 0; i < operations.Count; i++) {
				operations [i].applyOperation ();
			}
		}





	}
}

