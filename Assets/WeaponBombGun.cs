using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBombGun : MonoBehaviour {

	public GameObject projectile;
	public bool ready = true;
	public float cooldown = 2.0f;

	public float horizOffset = 1.0f;
	public float vertOffset = 0.0f;

	public float force = 1000.0f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void fire(GameObject player){

		Vector3 initialPos = player.transform.position + horizOffset * player.transform.forward;
		initialPos.y += vertOffset;

		//Quaternion.Euler(90, 0, 0)

		GameObject bomb = Instantiate (projectile, initialPos, player.transform.rotation * Quaternion.Euler(45, 0, 0));
		Rigidbody rb = bomb.GetComponent<Rigidbody>();
		rb.AddForce (force * player.transform.forward);
		//rb.AddExplosionForce(force, player.transform.position, 100.0f);



	}

}
