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
    private bool changing;
    public Vector3[] NextSpawnpoints;
    public bool isLastLevel;
    public GameObject LastlevelCanvas;



    // Update is called once per frame
    void Update()
    {
        if (pad1.Is_in && pad2.Is_in /*&& PhotonNetwork.IsMasterClient*/ && !changing)
        {
            changing = true;
            if(!isLastLevel)
                ChangeLevel();
            else
                LastLevel();
        }
    }
    
    void ChangeLevel()
    {
        Debug.Log("LevelChanger : Changing level");
        PhotonNetwork.LoadLevel(nextlevel);
        /*PlayerController[] pcs = FindObjectsOfType<PlayerController>();
        foreach(PlayerController pc in pcs)
        {
            pc.SetPlayerSpawnPoints(NextSpawnpoints);
            pc.transform.position = NextSpawnpoints[pc.playerNumber];
        }*/
    }
    void LastLevel()
    {
        LastlevelCanvas.SetActive(true);
    }

}
