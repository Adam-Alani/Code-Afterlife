using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LevelChanger : MonoBehaviour
{
    public Pad pad1;
    public Pad pad2;
    public int nextlevel;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pad1.Is_in && pad2.Is_in)
            ChangeLevel();
    }



    void ChangeLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");

            PhotonNetwork.LoadLevel(nextlevel);
        }
    }
}
