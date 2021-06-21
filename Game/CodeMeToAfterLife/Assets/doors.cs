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
    private bool isInRange; // if the player is in range ofthe door 
    public bool isAuto; // if the door is automatic or not



    // Start is called before the first frame update
    void Start()
    {
       animator.SetBool("Open", false);
       if (isAuto)
       {
           codeEditor = null;
       }
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
        
        if (isAuto)
            res = isInRange;
         else
            res = codeEditor.GetComponent<CodeEditor>().Solved;

        if (res && !prevres)
        {
            animator.SetBool("Open", res);
            FindObjectOfType<AudioManager>().Play("DoorOpen");
            Debug.Log("Played DoorOpen");
            if (!isAuto && !(redWire is null) && !(greenWire is null))
            {
                redWire.SetActive(false);
                greenWire.SetActive(true);
            }
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (isAuto && collider.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Door : PLAYER in RANGE");
        }
    }
}
