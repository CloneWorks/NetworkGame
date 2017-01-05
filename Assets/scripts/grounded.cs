using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grounded : MonoBehaviour {

    public float distToGround = 0;
    public float jumpForce = 1000;

    public Rigidbody rigid;
 
    void Start(){
        // get the distance to ground
        distToGround = GetComponent<Collider>().bounds.extents.y;

        //get rigid body
        rigid = GetComponent<Rigidbody>();
    }
 
    bool IsGrounded(){
        return Physics.Raycast(this.transform.position, -Vector3.up, distToGround - 0.7f);
    }
 
    void Update () {
         if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
         {
             Debug.Log("here");
             rigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
         }
    }

}
