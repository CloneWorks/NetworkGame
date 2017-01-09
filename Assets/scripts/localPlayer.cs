using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class localPlayer : NetworkBehaviour {

    [SyncVar] //pushes this out to all clients from the server (server to client)
    public string playerName = "Player";

    [SyncVar]
    public Color playerColour = Color.white;

    //[SyncVar]
    //public Animator anim;

    public static Vector3 playerPosition = Vector3.zero;

    public static GameObject player = null;

    void OnGUI()
    {
        if(isLocalPlayer){
            //playerName = GUI.TextField(new Rect(25, Screen.height - 40, 100, 30), playerName);
            //if (GUI.Button(new Rect(130, Screen.height - 40, 80, 30), "change"))
            //{
            //    CmdChangeName(playerName);
            //}
        }
    }

    [Command] //this function should be called on the server (client to server)
    public void CmdChangeName(string newName)
    {
        playerName = newName;
    }

	// Use this for initialization
	void Start () {
	    if(isLocalPlayer){
            GetComponent<playerController>().enabled = true;
            
            player = gameObject;

            //GetComponent<playerCamera>().enabled = true;

            GetComponent<IK_WallClimb>().enabled = true;
 
        }

        Renderer rend = GetComponentInChildren<Renderer>();

        rend.material.SetColor("_OutlineColor", playerColour);

        this.transform.position = new Vector3(Random.Range(-20,20), 0  , Random.Range(-20,20));
	}
	
	// Update is called once per frame
	void Update () {
	    //if(isLocalPlayer){
        this.GetComponentInChildren<TextMesh>().text = playerName;

        //anim = this.GetComponent<Animator>();

        //update local players position
        playerPosition = transform.position;

        //}
	}
}
