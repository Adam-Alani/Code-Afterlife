using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class doors : MonoBehaviour
{
    public Animator animator;
    public GameObject codeEditor;

    public GameObject greenWire;
    public GameObject redWire;

    private bool prevres;
    private bool res;



    // Start is called before the first frame update
    void Start()
    {
       animator.SetBool("Open", false);
    }

    // Update is called once per frame
    void Update()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("UpdateRpc", RpcTarget.All);
    }

    [PunRPC]
    void UpdateRpc()
    {
        prevres = res;
        res = codeEditor.GetComponent<CodeEditor>().Solved;

        if (res)
        {
            animator.SetBool("Open", res);
            if (!prevres)
            {
                FindObjectOfType<AudioManager>().Play("DoorOpen");
                Debug.Log("Played DoorOpen");
            }
            redWire.SetActive(false);
            greenWire.SetActive(true);
        }
    }
}
