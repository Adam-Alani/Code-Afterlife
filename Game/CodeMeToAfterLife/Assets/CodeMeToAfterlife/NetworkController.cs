using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class NetworkController : MonoBehaviourPunCallbacks
{
    /*****************************************************
     *
     * Documentation : https://doc.photonengine.com/en-us/pun/current/getting-started/pun-intro
     * Scripting API : https://doc-api.photonengine.com/en/pun/v2/index.html
     * 
     *****************************************************/
    
    
    // Start is called before the first frame update
    /// <summary>
    /// Connects the player to the servers
    /// </summary>
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Informs us when connected to a server
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Now connected to the " + PhotonNetwork.CloudRegion + " server !");
    }
}
