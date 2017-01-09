using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNail : MonoBehaviour {

	float destroyAfter = 3.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){

		Destroy (gameObject, destroyAfter);

	}

}
