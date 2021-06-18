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

    // Start is called before the first frame update
    /// <summary>
    /// Asks to create a player each time someone is connecting
    /// </summary>
    void Start()
    {
        SetSpawnPoints();
        cameraPosition = FindObjectOfType<CameraPosition>();
        CreatePlayer();
    }
       
    /// <summary>
    /// Creates the player located in : Code-Afterlife\Game\CodeMeToAfterLife\Assets\Resources\Prefabs
    /// (needs to be in the Resources folder in a folder named as the first parameter)
    /// </summary>
    private void CreatePlayer()
    {
        int playerNumber;
        if (PhotonNetwork.IsMasterClient)
        {
            playerNumber = 0;
        }
        else
        {
            playerNumber = 1;
        }
        
        Debug.Log("Creating Player");
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), spawnPoints[playerNumber], Quaternion.identity);
        PlayerController pc = FindObjectOfType<PlayerController>();
        pc.SetPlayerNumber(playerNumber);
        pc.SetPlayerSpawnPoints(spawnPoints);
        cameraPosition.SetCameraTarget(player.transform);
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
