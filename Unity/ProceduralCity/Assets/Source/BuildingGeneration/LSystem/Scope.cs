using System;
using UnityEngine;

namespace ProceduralCity
{
	public class Scope
	{
		private float Sx, Sy, Sz;
		private float Tx, Ty, Tz;
		private GameObject gameObject;
		public Scope (GameObject g)
		{
			this.gameObject = g;
			Tx = 0;
			Ty = 0;
			Tz = 0;

			Sx = 0;
			Sy = 0;
			Sz = 0;
		}

		public void setScope(float x, float y, float z) {
			this.Sx = x;
			this.Sy = y;
			this.Sz = z;
		}

		public GameObject getGameObject() {
			return this.gameObject;
		}

		public float getSx() {
			return this.Sx;
		}

		public float getSy() {
			return this.Sy;
		}

		public float getSz() {
			return this.Sz;
		}

		public void setTranslation(float x, float y, float z) {
			this.Tx += x;
			this.Ty += y;
			this.Tz += z;
		}

		public float getTx() {
			return this.Tx;
		}

		public float getTy() {
			return this.Ty;
		}

		public float getTz() {
			return this.Tz;
		}

		public Vector3 getScopeWithScalar(float scalar =1) {
			Vector3 v = new Vector3 (this.Sx, this.Sy, this.Sz);
				v.Scale (new Vector3 (scalar, scalar, scalar));
			return v;
		}

		public Vector3 getTranslationWithScalar(float scalar =1) {
			Vector3 v = new Vector3 (this.Tx, this.Ty, this.Tz);
			v.Scale (new Vector3 (scalar, scalar, scalar));
			return v;
		}


	}
}

