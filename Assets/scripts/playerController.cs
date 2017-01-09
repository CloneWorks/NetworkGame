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
    [SerializeField]
    private float rotationDegreePerSecond = 120.0f;
    
    //private globals
    private float direction = 0.0f;
    private float speed = 0.0f;
    private float h = 0.0f;
    private float v = 0.0f;
    private AnimatorStateInfo stateInfo;


    //hashes
    private int m_locomotionId = 0;

    //jumping vars
    public float distToGround = 0;
    public float jumpForceUp = 100;
    public float jumpForceForward = 50;
    public float angle = 45;

    public Rigidbody rigid;

    public CharacterController charC;

	// Use this for initialization
	void Start () {
        //setup camera offset
        //offset = new Vector3(0, cameraHeight, -cameraDistance);

        //get the camera
        //mainCamera = Camera.main.transform;
       
        //update the cameras position
        //moveCam();

        m_locomotionId = Animator.StringToHash("Base Layer.Motion");
        //Get Animator Controller
        animator = GetComponent<Animator>();

        if(animator.layerCount >= 2){
            animator.SetLayerWeight(1,1);
        }

        // get the distance to ground
        distToGround = GetComponent<Collider>().bounds.extents.y;

        //get rigid body
        rigid = GetComponent<Rigidbody>();

        //get char controller
        charC = GetComponent<CharacterController>();
	}

    void Update()
    {
        
    }

	// Update is called once per frame
	void FixedUpdate () {

        if (animator && IsGrounded())
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //pull values from keyboard/controller
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            v = CrossPlatformInputManager.GetAxis("Vertical");

            //speed = new Vector2(h, v).sqrMagnitude;

            stickToWorldSpace(this.transform, Camera.main.transform, ref direction, ref speed);

            animator.SetFloat("speed", speed);
            animator.SetFloat("direction", direction, directionDampTime, Time.deltaTime); //animator.SetFloat("direction", h, directionDampTime, Time.deltaTime);

        }

        //turning sharply
        if (animator.GetFloat("direction") > 2.75 || animator.GetFloat("direction") < -2.75)
        {
            animator.SetBool("sharpTurn", true);
        }
        else
        {
            animator.SetBool("sharpTurn", false);
        }

        if (animator.GetFloat("direction") == -5)
        {
            animator.SetBool("sharpTurn", false);
        }

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

        if(IsGrounded() && isInLocomotion() && ((direction >= 0 && h >= 0) || (direction < 0 && h < 0)))
        {
            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (h < 0f ? -1f : 1f), 0f), Mathf.Abs(h));
            Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            this.transform.rotation = (this.transform.rotation * deltaRotation);
        }

        //==================
        //jumping
        //==================
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !animator.GetCurrentAnimatorStateInfo(0).IsName("falling_flat_impact") && !animator.GetCurrentAnimatorStateInfo(0).IsName("getting_up") && animator.GetBool("jumping") == false)
        {
            animator.SetBool("jumping", true);
            animator.applyRootMotion = false;

            //rigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            //rigid.AddForce(new Vector3(0, 0, jumpForce), ForceMode.Impulse);

            //Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            //rigid.AddForce(dir * jumpForce);

            if(animator.GetFloat("speed") > 0.5)
            {
                rigid.AddForce(((transform.forward * jumpForceForward) + (Vector3.up * jumpForceUp * 1.5f)), ForceMode.Impulse);
            }
            else
            {
                rigid.AddForce((Vector3.up * (jumpForceUp)), ForceMode.Impulse);
            }
            
        }


        
    }

    void OnCollisionEnter(Collision collision)
    {
        //landing on the ground
        if (collision.gameObject.name == "Terrain")
        {
            //on the ground
            if (animator.GetBool("jumping") == true)
            {
                animator.SetBool("jumping", false);
                animator.applyRootMotion = true;
            }

            //on ground after explosion
            if (animator.GetBool("explode") == true)
            {
                animator.SetBool("explode", false);
                animator.applyRootMotion = true;
            }
        }

        //hits a mine
        if(collision.gameObject.tag == "explosive"){
            animator.SetBool("explode", true);
            animator.applyRootMotion = false;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(this.transform.position, -Vector3.up, distToGround + 0.0f);
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
				
        Vector3 stickDirection = new Vector3(h, 0, v);
		
		speedOut = stickDirection.sqrMagnitude;		

        // Get camera rotation
        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(CameraDirection));

        // Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
		
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.5f, root.position.z), axisSign, Color.red);
		
		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		
		angleRootToMove /= 180f;
		
		directionOut = angleRootToMove * directionSpeed;
	}	

    public bool isInLocomotion(){
        return stateInfo.nameHash == m_locomotionId;
    }

}
