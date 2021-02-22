using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    /// <summary>
    /// Asks to create a player each time someone is connecting
    /// </summary>
    void Start()
    {
        CreatePlayer();
    }
    /// <summary>
    /// Creates the player located in : Code-Afterlife\Game\CodeMeToAfterLife\Assets\Resources\Prefabs
    /// (needs to be in the Resources folder in a folder named as the first parameter)
    /// </summary>
    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), Vector3.zero, Quaternion.identity);
    }
}
