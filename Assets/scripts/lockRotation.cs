using UnityEngine;
using System.Collections;

public class lockRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
	}
}
