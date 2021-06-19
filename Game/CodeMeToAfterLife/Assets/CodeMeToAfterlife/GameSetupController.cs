using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class GameSetupController : MonoBehaviour
{
    CameraPosition cameraPosition;
    public Vector3[] spawnPoints;
    private int[] changeIndex = new int[] {1, 0};

    public List<int> SpawnPointsUsed = new List<int>();

    // Start is called before the first frame update
    /// <summary>
    /// Asks to create a player each time someone is connecting
    /// </summary>
    void Start()
    {
        SetSpawnPoints();
        cameraPosition = FindObjectOfType<CameraPosition>();
        //CreatePlayer();
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("CreatePlayer", RpcTarget.All);
    }
       
    /// <summary>
    /// Creates the player located in : Code-Afterlife\Game\CodeMeToAfterLife\Assets\Resources\Prefabs
    /// (needs to be in the Resources folder in a folder named as the first parameter)
    /// </summary>
    [PunRPC]
    private void CreatePlayer()
    {

        int playerNumber;
        if (SpawnPointsUsed.Count == 0)
        {
            playerNumber = new System.Random().Next(2);
            Debug.Log("GameSetupController : Random Choice");
        }
        else
        {
            playerNumber = changeIndex[SpawnPointsUsed[0]];
            Debug.Log($"GameSetupController : other player's number {SpawnPointsUsed[0]} and new one : {playerNumber}");
        }
        SpawnPointsUsed.Add(playerNumber);
        Debug.Log("Creating Player");
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), spawnPoints[playerNumber], Quaternion.identity);
        PlayerController pc = FindObjectOfType<PlayerController>();
        pc.SetPlayerNumber(playerNumber);
        pc.SetPlayerSpawnPoints(spawnPoints);
        cameraPosition.SetCameraTarget(player.transform);
    }

    public void RemovePlayer(int playerNumber)
    {
        SpawnPointsUsed.Remove(playerNumber);
    }

    /*
    /// <summary>
    /// Creates the player located in : Code-Afterlife\Game\CodeMeToAfterLife\Assets\Resources\Prefabs
    /// (needs to be in the Resources folder in a folder named as the first parameter)
    /// </summary>
    private void CreatePlayer()
    {
        int playerNumber;
        if (PhotonNetwork.IsMasterClient)
        {
            playerNumber = new System.Random().Next(2);
        }
        else
        {
            int OtherPlayerIndex = PhotonNetwork.masterClient().FindObjectOfType<PlayerController>().playerNumber;
            playerNumber = changeIndex[OtherPlayerIndex];
        }
        
        Debug.Log("Creating Player");
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), spawnPoints[playerNumber], Quaternion.identity);
        PlayerController pc = FindObjectOfType<PlayerController>();
        pc.SetPlayerNumber(playerNumber);
        pc.SetPlayerSpawnPoints(spawnPoints);
        cameraPosition.SetCameraTarget(player.transform);
    }*/



    private void SetSpawnPoints()
    {
        Vector3 spawnPoint1 = new Vector3(-20, 5, 40);
        Vector3 spawnPoint2 = new Vector3(-20, 5, -40);
        spawnPoints = new Vector3[] {spawnPoint1, spawnPoint2};
    }
}
