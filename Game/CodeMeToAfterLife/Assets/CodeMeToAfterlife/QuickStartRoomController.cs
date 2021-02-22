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

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        StartGame();
    }

    /// <summary>
    /// One
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
