using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeOnContact : MonoBehaviour {

	public float radius = 5.0f;
	public float power = 10.0f;

    public AudioSource audio;
    public Renderer rend;
    public ParticleSystem ps;
    public Rigidbody rigid = null;

    public bool destroyAfterTime = false;
    public float destroyAfter = 3.0f;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        rigid = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
        if(destroyAfterTime)
        {
            Destroy(gameObject, destroyAfter);
        }
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

                if(rb.transform.tag == "Player")
                {
                    //Tell player they've been hit by an explosion
                    rb.gameObject.GetComponent<playerController>().explode();
                }
			}
		}

        if(rigid)
        {
            rigid.isKinematic = true;
        }
        
        rend.enabled = false;
		Destroy(gameObject, 0.8f);
	}

}
