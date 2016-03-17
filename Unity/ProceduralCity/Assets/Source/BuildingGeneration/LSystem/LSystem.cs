using System;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class LSystem
	{
		//A list of rules, in order of execution.
		private List<Rule> rules;

		private List<Symbol> state;

		private int iterations;

		//Axiom is the starting point, it is the footprint of the buildings.
		private Axiom axiom;

		public LSystem (Axiom a, int iterations)
		{
			this.axiom = a;
			this.iterations = iterations;
			this.rules = new List<Rule> ();
			this.state = new List<Symbol> ();
			state.Add (axiom);
		}

		public LSystem add(Axiom a) {
			this.axiom = a;
			return this;
		}

		public LSystem add(Rule r) {
			return this.addRule (r);
		}

		private LSystem addRule(Rule r) {
			this.rules.Add (r);
			return this;
		}

		public LSystem executeRules(int itrts = 1) {
			//Apply all iterations
			for (int i = 0; i < itrts; i++) {
				for (int j = 0; j < state.Count; j++) {
					for (int k = 0; k < rules.Count; k++) {
						if (state [j].getName () == rules [k].getPredeccessorType ()) {
							rules [k].execute (state [j]);
						}
					}
				}
			}
			return this;
		}
	}
}

