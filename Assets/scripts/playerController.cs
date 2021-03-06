﻿using UnityEngine;
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

	public int currentWeapon = 0;
	public GameObject[] weapons;

    public Transform weaponPos;

    public GameObject weapon;

    public bool useIK = true;
    public Transform leftHandWeaponPos = null;
    public Transform rightHandWeaponPos = null;

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

        //get weapon position
        weaponPos = transform.FindChild("weaponPos");

        //equipt weapon
        equiptWeapon();
	}

	void Update(){


		for (int i = 1; i < 5; i++) {
			if (Input.GetKeyDown ("" + i) && !falling()) {
				currentWeapon = i - 1;
                equiptWeapon();
				Debug.Log (currentWeapon);
			}
		}


		//==================
		//Shooting
		//==================
		if (Input.GetMouseButtonDown (0) && !falling()) {
			GameObject gun = (GameObject)weapons [currentWeapon];


			//should be done through polymorphism - way simpler code here and in weapons
			if (currentWeapon == 0) {
				gun.GetComponent<WeaponNailgun> ().fire (gameObject);
			}
			if (currentWeapon == 1) {
				gun.GetComponent<WeaponMineGun> ().fire (gameObject);
			}
			if (currentWeapon == 2) {
				gun.GetComponent<WeaponBombGun> ().fire (gameObject);
			}
		}

	}

	// Update is called once per frame
	void FixedUpdate () {

		if (animator && IsGrounded ()) {
			stateInfo = animator.GetCurrentAnimatorStateInfo (0);

			//pull values from keyboard/controller
			h = CrossPlatformInputManager.GetAxis ("Horizontal");
			v = CrossPlatformInputManager.GetAxis ("Vertical");

			//speed = new Vector2(h, v).sqrMagnitude;

			stickToWorldSpace (this.transform, Camera.main.transform, ref direction, ref speed);

			animator.SetFloat ("speed", speed);
			animator.SetFloat ("direction", direction, directionDampTime, Time.deltaTime); //animator.SetFloat("direction", h, directionDampTime, Time.deltaTime);

		}

		//turning sharply
		if (animator.GetFloat ("direction") > 2.75 || animator.GetFloat ("direction") < -2.75) {
			animator.SetBool ("sharpTurn", true);
		} else {
			animator.SetBool ("sharpTurn", false);
		}

		if (animator.GetFloat ("direction") == -5) {
			animator.SetBool ("sharpTurn", false);
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

		if (IsGrounded () && isInLocomotion () && ((direction >= 0 && h >= 0) || (direction < 0 && h < 0))) {
			Vector3 rotationAmount = Vector3.Lerp (Vector3.zero, new Vector3 (0f, rotationDegreePerSecond * (h < 0f ? -1f : 1f), 0f), Mathf.Abs (h));
			Quaternion deltaRotation = Quaternion.Euler (rotationAmount * Time.deltaTime);
			this.transform.rotation = (this.transform.rotation * deltaRotation);
		}

		//==================
		//jumping
		//==================
		if (Input.GetKeyDown (KeyCode.Space) && IsGrounded () && !falling() && animator.GetBool ("jumping") == false) {
			animator.SetBool ("jumping", true);
			animator.applyRootMotion = false;

			//rigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
			//rigid.AddForce(new Vector3(0, 0, jumpForce), ForceMode.Impulse);

			//Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
			//rigid.AddForce(dir * jumpForce);

			if (animator.GetFloat ("speed") > 0.5) {
				rigid.AddForce (((transform.forward * jumpForceForward) + (Vector3.up * jumpForceUp * 1.5f)), ForceMode.Impulse);
			} else {
				rigid.AddForce ((Vector3.up * (jumpForceUp)), ForceMode.Impulse);
			}
            
		}

        //don't hold weapon when falling
        if (falling())
        {
            //take hands off weapon
            useIK = false;

            //let weapon fall
            weapon.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            //put hands back on weapon
            useIK = true;

            //pick weapon up
            weapon.GetComponent<Rigidbody>().isKinematic = true;
            equiptWeapon();
            
        }







	}








    void OnCollisionStay(Collision collision)
    {
        if(falling())
        {
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
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //landing on the ground
        //if (collision.gameObject.name == "Terrain") //may want to add tags for buildings and walls to avoid infinite jumping.
        //{
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
        //}

        //hits a mine
        if(collision.gameObject.tag == "explosive"){
            explode();
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(this.transform.position, -Vector3.up, distToGround + 0.0f);
    }

    public void explode()
    {
        animator.SetBool("explode", true);
        animator.applyRootMotion = false;
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

    public void equiptWeapon()
    {
        //equipt weapon
        Destroy(weapon);
        weapon = Instantiate(weapons[currentWeapon], new Vector3(weaponPos.position.x, weaponPos.position.y, weaponPos.position.z), weaponPos.rotation);
        weapon.transform.parent = gameObject.transform;

        //get hand positions from weapon prefab
        if(weapon.transform.childCount == 1)
        {
            leftHandWeaponPos = weapon.transform.GetChild(0).transform.FindChild("leftHand");
            rightHandWeaponPos = weapon.transform.GetChild(0).transform.FindChild("rightHand");
        }
        else
        {
            leftHandWeaponPos = weapon.transform.FindChild("leftHand");
            rightHandWeaponPos = weapon.transform.FindChild("rightHand");
        }
    }

    void OnAnimatorIK()
    {
        //position hands
        if (leftHandWeaponPos != null && rightHandWeaponPos != null && useIK)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandWeaponPos.position);
            //anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
            //anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandWeaponPos.position);
            //anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            //anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
        }
    }

    bool falling()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("falling") || animator.GetCurrentAnimatorStateInfo(0).IsName("falling_flat_impact") || animator.GetCurrentAnimatorStateInfo(0).IsName("getting_up"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
