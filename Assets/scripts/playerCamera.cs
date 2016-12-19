using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class playerCamera : NetworkBehaviour {

    //variables
    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform follow = null;
    [SerializeField]
    private Vector3 offset = new Vector3(0f, 1.5f, 0f);

    private Vector3 targetPosition = Vector3.zero;

    private Vector3 lookDir;

    private Vector3 velocityCamSmooth = Vector3.zero;
    [SerializeField]
    private float camSmoothDampTime = 0.1f;

	// Use this for initialization
	void Start () {
        //targetPosition = localPlayer.playerPosition;

        if(localPlayer.player != null){
            follow = localPlayer.player.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //targetPosition = localPlayer.playerPosition;

        if (follow == null && localPlayer.player != null)
        {
            follow = localPlayer.player.transform;
        }

	}

    void LateUpdate()
    {
        if(follow != null)
        {
            Vector3 characterOffset = follow.position + offset;

            lookDir = characterOffset - this.transform.position;
            lookDir.y = 0;
            lookDir.Normalize();
            Debug.DrawRay(this.transform.position, lookDir, Color.white);

            targetPosition = characterOffset + follow.up * distanceUp - lookDir * distanceAway;

            //targetPosition = follow.position + follow.up * distanceUp - follow.forward * distanceAway;
            //Debug.DrawRay(follow.position, follow.up * distanceUp, Color.red);
            //Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
            Debug.DrawLine(follow.position, targetPosition, Color.magenta);

            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
            smoothPosition(this.transform.position, targetPosition);

            transform.LookAt(follow);
        }
    }

    private void smoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }
}
