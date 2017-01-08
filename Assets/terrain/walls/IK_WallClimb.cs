using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_WallClimb : MonoBehaviour {

    public bool useIK = true; //do you want to use IK or not.

    public bool leftHand;
    public bool rightHand;

    public bool leftFoot;
    public bool rightFoot;

    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    public Quaternion leftHandRot;
    public Quaternion rightHandRot;

    Animator anim;

	// Use this for initialization
	void Start () {
        //get animator component
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + new Vector3(0.0f, 1.2f, 0.4f), -transform.up + new Vector3(-0.5f, 0.0f, 0.0f), Color.blue, 0.0f);
        Debug.DrawRay(transform.position + new Vector3(0.0f, 1.2f, 0.4f), -transform.up + new Vector3(0.5f, 0.0f, 0.0f), Color.red, 0.0f);
    }

    void FixedUpdate()
    {
        RaycastHit leftHit;
        RaycastHit rightHit;

        if(Physics.Raycast(transform.position + new Vector3(0.0f, 1.2f, 0.4f), -transform.up + new Vector3(-0.5f, 0.0f, 0.0f), out leftHit, 1f))
        {
            leftHand = true;
            leftHandPos = leftHit.point;
            leftHandRot = Quaternion.FromToRotation(Vector3.forward, leftHit.normal);
        }
        else
        {
            leftHand = false;
        }

        if (Physics.Raycast(transform.position + new Vector3(0.0f, 1.2f, 0.4f), -transform.up + new Vector3(0.5f, 0.0f, 0.0f), out rightHit, 1f))
        {
            rightHand = true;
            rightHandPos = rightHit.point;
            rightHandRot = Quaternion.FromToRotation(Vector3.forward, rightHit.normal);
        }
        else
        {
            rightHand = false;
        }

    }

    void OnAnimatorIK(){
        if(useIK){
            if(leftHand)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
            }

            if (rightHand)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
                anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
            }
        }
    }
}
