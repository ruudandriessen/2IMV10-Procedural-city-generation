using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		public List<Symbol> executeRules() {
			List<Symbol> currentShapes = new List<Symbol> ();
			currentShapes = state;
			//Apply all iterations
			for (int i = 0; i < iterations; i++) {
				//Debug.Log ("----------------------");
				//Debug.Log ("Iteration: " + i);
				//Debug.Log ("Shapes: " + currentShapes.Count);
				for (int x = 0; x < currentShapes.Count; x++) {
					//Debug.Log(currentShapes[x].getName());
				}
				List<Symbol> nextState = new List<Symbol> ();
				for (int j = 0; j < currentShapes.Count; j++) {
					if (currentShapes [j].isFinal ()) {
						nextState.Add (currentShapes[j]);
					}
					if (!currentShapes [j].getIsEvaluated ()) {
						for (int k = 0; k < rules.Count; k++) {
							//Debug.Log ((currentShapes [j].getName () == rules [k].getPredeccessorType ()) + ", " + currentShapes [j].getName () + ", " + rules [k].getPredeccessorType ()); 
							if (currentShapes [j].getName () == rules [k].getPredeccessorType ()) {
								rules [k].execute (currentShapes [j], ref nextState);
							}
						}
						currentShapes [j].evaluate ();
					}
				}
				currentShapes = nextState;
			}
			/*Debug.Log (currentShapes.Count);
			for (int i = 0; i < currentShapes.Count; i++) {
				Debug.Log(currentShapes[i].extraValues["center"]);
			}*/
			//Debug.Log (currentShapes [1].extraValues ["center"]);
			return currentShapes;
		}
	}
}

