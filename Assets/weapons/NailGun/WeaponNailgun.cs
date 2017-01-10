using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponNailgun : MonoBehaviour {

	public GameObject projectile;

	public bool ready = true;
	public float cooldown = 2.0f;

	public float horizOffset = 1.0f;
	public float vertOffset = 0.0f;

	public float force = 10000.0f;

    public Transform weaponPos;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void fire(GameObject player){

        weaponPos = player.transform.FindChild("weaponPos");

		Vector3 initialPos = weaponPos.transform.position + horizOffset * player.transform.forward;
		initialPos.y += vertOffset;

		//Quaternion.Euler(90, 0, 0)

		GameObject nail = Instantiate (projectile, initialPos, player.transform.rotation * Quaternion.Euler(90, 0, 0));
		Rigidbody rb = nail.GetComponent<Rigidbody>();
		rb.AddForce (force * player.transform.forward);
		//rb.AddExplosionForce(force, player.transform.position, 100.0f);



	}

}
