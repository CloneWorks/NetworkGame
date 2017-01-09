using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMineGun : MonoBehaviour {

	public GameObject projectile;
	public bool ready = true;
	public float cooldown = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void fire(){
		Debug.Log ("fire called");
	}

}
