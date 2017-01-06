using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeOnContact : MonoBehaviour {

	public float radius = 5.0f;
	public float power = 10.0f;

    public AudioSource audio;
    public Renderer rend;
    public ParticleSystem ps;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(){

		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

            audio.Play();
            ps.Play();

			if (rb != null) {
				rb.AddExplosionForce (power, explosionPos, radius, 3.0F);
			}
		}

        //rend.enabled = false;
		Destroy(gameObject, 1.0f);
	}

}
