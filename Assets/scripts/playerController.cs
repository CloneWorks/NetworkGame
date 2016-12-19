using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class playerController : MonoBehaviour {
    //===================================
    //Variables
    //===================================
    //inspector variables
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float directionDampTime = 0.05f;
    [SerializeField]
    private float directionSpeed = 3.0f;
    
    //private globals
    private float direction = 0.0f;
    private float speed = 0.0f;
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

    void Update()
    {
        if (animator)
        {
            //pull values from keyboard/controller
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            v = CrossPlatformInputManager.GetAxis("Vertical");

            //speed = new Vector2(h, v).sqrMagnitude;

            stickToWorldSpace(this.transform, Camera.main.transform, ref direction, ref speed);

            animator.SetFloat("speed", speed);
            animator.SetFloat("direction", direction, directionDampTime, Time.deltaTime); //animator.SetFloat("direction", h, directionDampTime, Time.deltaTime);

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

        //if(isInLocomotion() && ((direction >= 0 && h >= 0) || (direction < 0 && h < 0)))
        //{

        //}
    }

    //public void moveCam()
    //{
     //   mainCamera.position = transform.position + new Vector3(0, 4, 0);
    //    mainCamera.rotation = transform.rotation;
     //   mainCamera.rotation *= Quaternion.Euler(cameraRotation, 0, 0);
     //   mainCamera.Translate(offset);
     //   mainCamera.LookAt(transform);
    //}

    public void stickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(h, 0 , v);

        speedOut = stickDirection.sqrMagnitude;

        //get camera rotation
        Vector3 cameraDirection = camera.forward;

        cameraDirection.y = 0.0f;

        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

        //convert joystick to world space
        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.grey);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.cyan);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.yellow);

        angleRootToMove /= 180;

        directionOut = angleRootToMove * directionSpeed;

    }
}
