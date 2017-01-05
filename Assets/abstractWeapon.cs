using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class abstractWeapon : MonoBehaviour {

	public int ammo;

	public abstract int ammoCapacity { get; }
	public abstract float fireRate { get; }
	public abstract float reloadDelay { get; }
	public abstract float damage { get; }
	/*
		derived:
		public override string MyConst {
    		get { return "constant"; }
		}
	*/

	public void fire(){
		
	}

}
