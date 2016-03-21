using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class RepeatOperation : Operation
	{
		string output;
		string length;
		string dimension;
		public RepeatOperation (Scope g, string dimension, string length, string output) : base(g)
		{
			this.output = output;
			this.length = length;
			this.dimension = dimension;
		}

		public override void applyOperation(Symbol s, ref List<Symbol> symbols) {
			Debug.Log("Repeat Operation on "+ dimension + " parameters and " + output + " as output");
			List<Vector3> points = s.getPoints ();
			Color c = UnityEngine.Random.ColorHSV();
			float distance = 0;
			if (dimension == "X") {
				distance = (points [0] - points [1]).magnitude;
			}
			else if(dimension == "Y") {
				distance = (points [0] - points [2]).magnitude;
			}
			int repetitions = (int)Math.Ceiling ((double)distance / float.Parse (this.length));
			float repDistance = distance / repetitions;
			float oldFraction = 0;
			Debug.Log ("Repetitions: " + repetitions + " , RepDistance: " + repDistance);
			for (int i = 0; i < repetitions; i++) {
				Vector3[] newPoints = new Vector3[4];
				float fraction = (oldFraction * distance + repDistance) / distance;
				if (dimension == "X") {
					newPoints [0] = points [0] * oldFraction + points [1] * (1 - oldFraction);
					newPoints [1] = points [0] * fraction + points [1] * (1 - fraction);
					newPoints [2] = points [2] * oldFraction + points [3] * (1 - oldFraction);
					newPoints [3] = points [2] * fraction + points [3] * (1 - fraction);
					Debug.DrawLine (newPoints [0], newPoints [1], c, 2000f);
					Debug.DrawLine (newPoints [2], newPoints [3], c, 2000f);
					Debug.DrawLine (newPoints [1], newPoints [3], c, 2000f);
				} else if (dimension == "Y") {
					newPoints [2] = points [0] * oldFraction + points [2] * (1 - oldFraction);
					newPoints [0] = points [0] * fraction + points [2] * (1 - fraction);
					newPoints [3] = points [1] * oldFraction + points [3] * (1 - oldFraction);
					newPoints [1] = points [1] * fraction + points [3] * (1 - fraction);
					Debug.DrawLine (newPoints [0], newPoints [1], c, 2000f);
					Debug.DrawLine (newPoints [2], newPoints [3], c, 2000f);
					Debug.DrawLine (newPoints [1], newPoints [3], c, 2000f);
				}
				oldFraction = fraction;

				symbols.Add (new Symbol (output, newPoints));
			}
				/*float rDistance = solveR (distance, parameters);
				Debug.Log ("Distance :" + distance);
				Debug.Log ("RDISTANCE = " + rDistance);
				float oldFraction = 0;
				for (int i = 0; i < parameters.Length; i++) {
					float value;
					if (parameters [i].EndsWith ("r")) {
						value = float.Parse (parameters [i].Substring (0, parameters [i].Length - 1)) * rDistance;
					} else {
						value = float.Parse (parameters [i]);
					}
					Vector3[] newPoints = new Vector3[4];
					float fraction = (oldFraction * distance + value) / distance;
					newPoints [0] = points [0] * oldFraction + points [1] * (1 - oldFraction);
					newPoints [1] = points [0] * fraction + points [1] * (1 - fraction);
					newPoints [2] = points [2] * oldFraction + points [3] * (1 - oldFraction);
					newPoints [3] = points [2] * fraction + points [3] * (1 - fraction);
					Debug.DrawLine (newPoints [0], newPoints [1], c, 2000f);
					Debug.DrawLine (newPoints [2], newPoints [3], c, 2000f);
					Debug.DrawLine (newPoints [1], newPoints [3], c, 2000f);
					oldFraction = fraction;
					symbols.Add (new Symbol (parameters [i], newPoints));

				}
			} else if (dimension == "Y") {
				float distance = (points [0] - points [2]).magnitude;
				float rDistance = solveR (distance, parameters);
				Debug.Log ("Distance :" + distance);
				Debug.Log ("RDISTANCE = " + rDistance);
				float oldFraction = 0;
				for (int i = 0; i < parameters.Length; i++) {
					float value;
					if (parameters [i].EndsWith ("r")) {
						value = float.Parse (parameters [i].Substring (0, parameters [i].Length - 1)) * rDistance;
					} else {
						value = float.Parse (parameters [i]);
					}
					Vector3[] newPoints = new Vector3[4];
					float fraction = (oldFraction * distance + value) / distance;
					newPoints [0] = points [0] * oldFraction + points [2] * (1 - oldFraction);
					newPoints [1] = points [0] * fraction + points [2] * (1 - fraction);
					newPoints [2] = points [1] * oldFraction + points [3] * (1 - oldFraction);
					newPoints [3] = points [1] * fraction + points [3] * (1 - fraction);
					Debug.DrawLine (newPoints [0], newPoints [1], c, 2000f);
					Debug.DrawLine (newPoints [2], newPoints [3], c, 2000f);
					Debug.DrawLine (newPoints [1], newPoints [3], c, 2000f);
					oldFraction = fraction;
					symbols.Add (new Symbol (parameters [i], newPoints));

				}
			}*/
		}

		private float solveR(float distance, string[] parameters) {
			float numberOfRs = (float) parseParametersForRs(parameters);
			float numberOfNRs = parseParametersForNRs (parameters);
			float remaining = distance - numberOfNRs;
			return remaining / numberOfRs;
		}

		private int parseParametersForRs(string[] parameters) {
			int totalR = 0;
			for (int i = 0; i < parameters.Length; i++) {
				if(parameters[i].EndsWith("r")) {
					string value = parameters [i].Substring (0, parameters [i].Length - 1);
					totalR += int.Parse (value);
				}
			}
			return totalR;
		}

		private float parseParametersForNRs(string[] parameters) {
			float totalNR = 0;
			for (int i = 0; i < parameters.Length; i++) {
				if(!parameters[i].EndsWith("r")) {
					string value = parameters [i];
					totalNR += float.Parse (value);
				}
			}
			return totalNR;
		}
	}
}

