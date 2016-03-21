using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralCity
{
	public abstract class Module
	{
		public enum ModuleInput {corner, edge, region, model, cell, allEdges, allCorners, allRegions};
		protected List<Module> children;
		protected ModuleInput inputType;
		protected Transform parent;

		public Module () {
			children = new List<Module> ();
		}

		public Module (List<Module> children) {
			this.children = children;
		}

		public ModuleInput getInputType() {
			return inputType;
		}

		public void addChild(Module m){
			children.Add (m);
		}

		public List<Module> getChildren() {
			return children;
		}
	}
}

