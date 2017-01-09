using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_WallClimb : MonoBehaviour {

    public bool useIK = true; //do you want to use IK or not.
    public bool useFeetIK = true; //do you want feet
    public bool useHandIK = true; //do you want hands

    public bool leftHand;
    public bool rightHand;

    public bool leftFoot;
    public bool rightFoot;

    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    public Vector3 leftFootPos;
    public Vector3 rightFootPos;

    public Vector3 rightHandOffset;
    public Vector3 leftHandOffset;

    public Vector3 rightFootOffset;
    public Vector3 leftFootOffset;

    public Quaternion leftHandRot;
    public Quaternion rightHandRot;

    public Transform wallFinderRayCastPoint;

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

        Quaternion.AngleAxis(-15, wallFinderRayCastPoint.right);
        Debug.DrawRay(wallFinderRayCastPoint.position, -transform.up + -transform.right*0.5f, Color.blue, 0.0f);
        Debug.DrawRay(wallFinderRayCastPoint.position, -transform.up + transform.right*0.5f, Color.red, 0.0f);


        //Debug.DrawRay(wallFinderRayCastPoint.position, -transform.up + -wallFinderRayCastPoint.right, Color.blue, 0.0f);
        //Debug.DrawRay(wallFinderRayCastPoint.position, -transform.up + wallFinderRayCastPoint.right, Color.red, 0.0f);
    }

    void FixedUpdate()
    {
        RaycastHit leftHit;
        RaycastHit rightHit;

        //Ray for left hand
        if (Physics.Raycast(wallFinderRayCastPoint.position, -transform.up + -transform.right * 0.5f, out leftHit, 1f)) //if(Physics.Raycast(transform.position + new Vector3(0.0f, 1.2f, 0.4f), -transform.up + new Vector3(-0.5f, 0.0f, 0.0f), out leftHit, 1f))
        {
            if (leftHit.transform.tag == "wall")
            {
                leftHand = true;
                leftHandPos = leftHit.point - leftHandOffset;
                leftHandRot = Quaternion.FromToRotation(Vector3.forward, leftHit.normal);
            }
        }
        else
        {
            leftHand = false;
        }

        //ray for right hand
        if (Physics.Raycast(wallFinderRayCastPoint.position, -transform.up + transform.right * 0.5f, out rightHit, 1f)) //if (Physics.Raycast(transform.position + new Vector3(0.0f, 1.2f, 0.4f), -transform.up + new Vector3(0.5f, 0.0f, 0.0f), out rightHit, 1f))
        {
            if(rightHit.transform.tag == "wall")
            {
                rightHand = true;
                rightHandPos = rightHit.point - rightHandOffset;
                rightHandRot = Quaternion.FromToRotation(Vector3.forward, rightHit.normal);
            }
        }
        else
        {
            rightHand = false;
        }

        //ray for left foot
        if(Physics.Raycast(transform.position + new Vector3(-0.2f, 0.0f,0.0f), transform.forward, out leftHit, 0.4f))
        {
            if (leftHit.transform.tag == "wall")
            {
                leftFoot = true;
                leftFootPos = leftHit.point; //- leftFootOffset;
            }
        }
        else
        {
            leftFoot = false;
        }

        //ray for right foot
        if (Physics.Raycast(transform.position + new Vector3(0.2f, 0.0f, 0.0f), transform.forward, out rightHit, 0.4f))
        {
            if (rightHit.transform.tag == "wall")
            {
                rightFoot = true;
                rightFootPos = rightHit.point; //- rightFootOffset;
            }
        }
        else
        {
            rightFoot = false;
        }
    }

    void OnAnimatorIK(){
        if(useIK){
            if(useHandIK)
            {
                if (leftHand)
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

            if (useFeetIK)
            {
                if (leftFoot)
                {
                    anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
                }

                if (rightFoot)
                {
                    anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
                }
            }
            
        }
    }
}
