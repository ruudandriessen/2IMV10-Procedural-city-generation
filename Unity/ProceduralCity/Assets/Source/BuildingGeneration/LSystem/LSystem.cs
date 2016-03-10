using System;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class LSystem
	{
		//A list of rules, in order of execution.
		private List<Rule> rules;

		//Axiom is the starting point, it is the footprint of the buildings.
		private Axiom axiom;

		public LSystem ()
		{
			rules = new List<Rule> ();
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
	}
}

