using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class drive : MonoBehaviour {

    float speed = 10.0f;
    float rotationSpeed = 100.0f;


    Transform mainCamera;
    Vector3 offset;
    public float cameraDistance = 1.0f;
    public float cameraHeight = 4.0f;
    public float cameraRotation = 5.0f;

	// Use this for initialization
	void Start () {
        //setup camera offset
        offset = new Vector3(0, cameraHeight, -cameraDistance);

        //get the camera
        mainCamera = Camera.main.transform;

       
        //update the cameras position
        moveCam();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    //if this is your player object
        float translation = CrossPlatformInputManager.GetAxis("Vertical") * speed;
        float rotation = CrossPlatformInputManager.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0,0, translation);
        transform.Rotate(0, rotation, 0);

        moveCam();
    }

    public void moveCam()
    {
        mainCamera.position = transform.position + new Vector3(0, 4, 0);
        mainCamera.rotation = transform.rotation;
        mainCamera.rotation *= Quaternion.Euler(cameraRotation, 0, 0);
        mainCamera.Translate(offset);
        mainCamera.LookAt(transform);
    }
}
