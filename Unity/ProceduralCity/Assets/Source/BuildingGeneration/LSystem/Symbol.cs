using System;
using UnityEngine;

namespace ProceduralCity
{
	public class Symbol
	{
		private string name;
		private string id;

		public Symbol (string name)
		{
			this.id = GetUniqueID ();
			this.name = name;
		}

		public bool hasId(String id) {
			return this.id == id;
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

			Debug.Log("Generated Unique ID: "+uniqueID);

			if(PlayerPrefs.HasKey(key)){
				uniqueID = PlayerPrefs.GetString(key);            
			} else {            
				PlayerPrefs.SetString(key, uniqueID);
				PlayerPrefs.Save();    
			}

			return uniqueID;
		}
	}
}

