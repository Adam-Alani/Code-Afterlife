using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Where the players will be moved for the first scene
    /// </summary>
    public int multyplayerSceneIndex;

    
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    
    /// <summary>
    /// Calls start game ones a player has joined a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        StartGame();
    }

    /// <summary>
    /// Loads the scene when the masterClient connect (the one who created the room)
    /// </summary>
    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multyplayerSceneIndex);
        }
    }
}
