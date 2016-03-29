using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class InsertModelOperation : Operation
	{
		string model;
		public InsertModelOperation (Scope g, string model) : base(g)
		{
			this.model = model;
		}

		public override void applyOperation(Symbol s, ref List<Symbol> symbols) {
			//Debug.Log ("Going to insert model " + this.model);
			GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			newObj.name = this.model;
			//GameObject newObj = new GameObject(this.model);
			if (this.model == "window") {
				newObj.transform.localScale = new Vector3 (1.5f, 0.1f, 2f);
				int random = new System.Random ().Next (0, 4);
				if (random == 0) {
					newObj.GetComponent<MeshRenderer> ().material.mainTexture = Resources.Load<Texture> ("Textures/HighRiseGlass0016_1_S");
				} else if (random == 1) {
					newObj.GetComponent<MeshRenderer> ().material.mainTexture = Resources.Load<Texture> ("Textures/HighRiseGlass0055_1_S");
				
				} else if (random == 2) {
					newObj.GetComponent<MeshRenderer> ().material.mainTexture = Resources.Load<Texture> ("Textures/WindowsBacklit0020_1_s");

				} else if (random == 3) {
					newObj.GetComponent<MeshRenderer> ().material.mainTexture = Resources.Load<Texture> ("Textures/WindowsBLocks0002_4_s");
				}
			} else if (this.model == "door") {
				newObj.transform.localScale = new Vector3 (2.5f, 0.05f, 4.5f);
				newObj.transform.Translate (0, 0.8f, 0);
				int random = new System.Random ().Next (0, 2);
				if (random == 0) {
				newObj.GetComponent<MeshRenderer> ().material.mainTexture = Resources.Load<Texture> ("Textures/DoorsWood0133_S");
				} else if (random == 1) {
					newObj.GetComponent<MeshRenderer> ().material.mainTexture = Resources.Load<Texture> ("Textures/DoorsWood0140_1_S");
				}
			}

			newObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			newObj.isStatic = true;

			Vector3[] symbolPoints = s.getPoints ().ToArray();
			Vector3 normal = Vector3.Cross ((symbolPoints [1] - symbolPoints [0]), (symbolPoints [2] - symbolPoints [0]));
			normal = normal / normal.magnitude;
			newObj.transform.Translate ((symbolPoints [0]+symbolPoints[1]+symbolPoints[2]+symbolPoints[3])/4);
			newObj.transform.Rotate (Vector3.left, 270);
			Quaternion _facing = newObj.transform.rotation;
			newObj.transform.rotation = Quaternion.LookRotation (normal)*_facing;
			if (this.model == "door") {
				//objMesh.transform.Translate(new Vector3(0.0f,-0.1f,0.75f));
			}


		}
	}
}

