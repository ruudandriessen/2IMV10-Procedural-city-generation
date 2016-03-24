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
			GameObject newObj = new GameObject("Window");
			newObj.AddComponent <MeshRenderer>();

			MeshFilter objMesh = newObj.AddComponent<MeshFilter>();
			Material[] materials = null;
			if (this.model == "window") {
				GameObject newMesh = Resources.Load ("window") as GameObject;
				objMesh.mesh = newMesh.GetComponent<MeshFilter> ().sharedMesh;
				materials = new Material[3];
				materials [0] = Resources.Load ("Materials/HARVEST GOLD PEARL UC106690F") as Material;
				materials [1] = Resources.Load ("Materials/Wood_Cherry") as Material;
				materials [2] = Resources.Load ("Materials/_Gray Glass_") as Material;
			} else if (this.model == "door") {
				GameObject newMesh = Resources.Load ("door") as GameObject;
				objMesh.mesh = newMesh.GetComponent<MeshFilter> ().sharedMesh;
				materials = new Material[2];
				materials [0] = Resources.Load ("Materials/dbl_door2.dxf_001") as Material;
				materials [1] = Resources.Load ("Materials/dbl_door2.dxf_002") as Material;
				objMesh.transform.localScale = new Vector3(1.5f,1f,1.5f);
			}
			objMesh.GetComponent<MeshRenderer> ().materials = materials;

			Vector3[] symbolPoints = s.getPoints ().ToArray();
			Vector3 normal = Vector3.Cross ((symbolPoints [1] - symbolPoints [0]), (symbolPoints [2] - symbolPoints [0]));
			normal = normal / normal.magnitude;
			objMesh.transform.Translate ((symbolPoints [0]+symbolPoints[1]+symbolPoints[2]+symbolPoints[3])/4);
			objMesh.transform.Rotate (Vector3.left, 270);
			Quaternion _facing = objMesh.transform.rotation;
			objMesh.transform.rotation = Quaternion.LookRotation (normal)*_facing;
			if (this.model == "door") {
				//objMesh.transform.Translate(new Vector3(0.0f,-0.1f,0.75f));
			}


		}
	}
}

