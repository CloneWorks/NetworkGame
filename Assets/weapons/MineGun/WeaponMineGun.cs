using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMineGun : MonoBehaviour {

	public GameObject projectile;
	public bool ready = true;
	public float cooldown = 2.0f;

	public float horizOffset = 1.0f;
	public float vertOffset = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void fire(GameObject player){

		Vector3 initialPos = player.transform.position + horizOffset * player.transform.forward;
		initialPos.y += vertOffset;
	
		GameObject mine = Instantiate(projectile, initialPos, Quaternion.identity);
	}

}
