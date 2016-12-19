using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class playerController : MonoBehaviour {
    //===================================
    //Variables
    //===================================

    //old vars before animator
    float speed = 0.1f;
    float rotationSpeed = 2.0f;

    //Transform mainCamera;
    //Vector3 offset;
    //public float cameraDistance = 1.0f;
    //public float cameraHeight = 4.0f;
    //public float cameraRotation = 5.0f;
    // before animator-->

    //new vars for animator
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float directionDampTime = 0.05f;

    private float speed2 = 0.0f;
    private float h = 0.0f;
    private float v = 0.0f;

	// Use this for initialization
	void Start () {
        //setup camera offset
        //offset = new Vector3(0, cameraHeight, -cameraDistance);

        //get the camera
        //mainCamera = Camera.main.transform;
       
        //update the cameras position
        //moveCam();

        //Get Animator Controller
        animator = GetComponent<Animator>();

        if(animator.layerCount >= 2){
            animator.SetLayerWeight(1,1);
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //-------------------Before animator -----------------------------------------------------------------
	    //if this is your player object
        //float translation = CrossPlatformInputManager.GetAxis("Vertical") * speed;
        //float rotation = CrossPlatformInputManager.GetAxis("Horizontal") * rotationSpeed;

        //translation *= Time.deltaTime;
        //rotation *= Time.deltaTime;

        //transform.Translate(0,0, translation);
        //transform.Rotate(0, rotation, 0);

        //moveCam();
        //-------------------Before animator -----------------------------------------------------------------

        if(animator){
            //pull values from keyboard/controller
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            v = CrossPlatformInputManager.GetAxis("Vertical");

            speed2 = new Vector2(h, v).sqrMagnitude;

            animator.SetFloat("speed", speed2);
            animator.SetFloat("direction", h, directionDampTime, Time.deltaTime);

            transform.Translate(0, 0, v * speed);
            transform.Rotate(0, h * rotationSpeed, 0);
        }

    }

    //public void moveCam()
    //{
     //   mainCamera.position = transform.position + new Vector3(0, 4, 0);
    //    mainCamera.rotation = transform.rotation;
     //   mainCamera.rotation *= Quaternion.Euler(cameraRotation, 0, 0);
     //   mainCamera.Translate(offset);
     //   mainCamera.LookAt(transform);
    //}
}
