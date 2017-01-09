using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class networkLobbyHook : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        localPlayer localPlayer = gamePlayer.GetComponent<localPlayer>();

        localPlayer.playerName = lobby.playerName;
        localPlayer.playerColour = lobby.playerColor;
    }
}
