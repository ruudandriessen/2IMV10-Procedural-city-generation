using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class Symbol
	{
		private string name;
		private string id;
		private List<Vector3> points;
		private bool finalShape;
		private bool isEvaluated = false;
		public Dictionary<string, Vector3> extraValues;

		public Symbol (string name, Vector3[] pointsA, bool isFinal = false)
		{
			this.points = new List<Vector3> ();
			this.id = GetUniqueID ();
			this.name = name;
			for (int i = 0; i < pointsA.Length; i++) {
				this.points.Add (pointsA [i]);
			}
			this.finalShape = isFinal;
			this.extraValues = new Dictionary<string, Vector3> ();
		}

		public bool hasId(String id) {
			return this.id == id;
		}

		public bool isFinal() {
			return this.finalShape;
		}

		public void evaluate() {
			this.isEvaluated = true;
		}

		public bool getIsEvaluated() {
			return this.isEvaluated;
		}

		public string getName() {
			return this.name;
		}

		private string GetUniqueID(){
			string key = "ID";

			var random = new System.Random();                     
			DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
			double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;

			string uniqueID = Application.systemLanguage                            //Language                                     //Device    
				+"-"+String.Format("{0:X}", Convert.ToInt32(timestamp))                //Time
				+"-"+String.Format("{0:X}", Convert.ToInt32(Time.time*1000000))        //Time in game
				+"-"+String.Format("{0:X}", random.Next(1000000000));                //random number

			//Debug.Log("Generated Unique ID: "+uniqueID);

			if(PlayerPrefs.HasKey(key)){
				uniqueID = PlayerPrefs.GetString(key);            
			} else {            
				PlayerPrefs.SetString(key, uniqueID);
				PlayerPrefs.Save();    
			}

			return uniqueID;
		}
		public void addPoint(Vector3 p) {
			this.points.Add (p);
		}

		public List<Vector3> getPoints() {
			return this.points;
		}

		public Vector3 getPoint(int i) {
			return this.points [i];
		}
	}
}

