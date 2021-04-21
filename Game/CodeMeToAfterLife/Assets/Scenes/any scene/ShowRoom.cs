using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShowRoom : MonoBehaviour
{
    public Text txt;
    

    // Start is called before the first frame update
    void Start()
    {
        txt.text = PhotonNetwork.CurrentRoom.Name;
    }

}
