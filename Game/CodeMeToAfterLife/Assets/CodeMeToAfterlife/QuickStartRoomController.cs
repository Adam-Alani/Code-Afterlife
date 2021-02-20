using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
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
        Debug.Log("Hoined Room");
        StartGame();
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multyplayerSceneIndex);
        }
    }
}
