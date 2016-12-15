using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class localPlayer : NetworkBehaviour {

    [SyncVar] //pushes this out to all clients from the server (server to client)
    public string playerName = "Player";

    void OnGUI()
    {
        if(isLocalPlayer){
            playerName = GUI.TextField(new Rect(25, Screen.height - 40, 100, 30), playerName);
            if (GUI.Button(new Rect(130, Screen.height - 40, 80, 30), "change"))
            {
                CmdChangeName(playerName);
            }
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
            GetComponent<drive>().enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    //if(isLocalPlayer){
        this.GetComponentInChildren<TextMesh>().text = playerName;
        //}
	}
}
