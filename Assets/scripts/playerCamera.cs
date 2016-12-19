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
    private Vector3 targetPosition = Vector3.zero;

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
            targetPosition = follow.position + follow.up * distanceUp - follow.forward * distanceAway;
            Debug.DrawRay(follow.position, follow.up * distanceUp, Color.red);
            Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
            Debug.DrawLine(follow.position, targetPosition, Color.magenta);

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

            transform.LookAt(follow);
        }
    }
}
