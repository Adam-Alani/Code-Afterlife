﻿using System.Collections;
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
        if (res)
            return;
        
        if (isAuto)
            res = isInRange;
        else
            res = codeEditor.GetComponent<CodeEditor>().Solved;
        
        if (res)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("Disable", RpcTarget.All); 
        }

    }

    [PunRPC] 
    void Disable()
    {
        res = true;
        animator.SetBool("Open", true);
        FindObjectOfType<AudioManager>().Play("DoorOpen");
        Debug.Log("Played DoorOpen");
        if (!isAuto && !(redWire is null || greenWire is null))
        {
            redWire.SetActive(false);
            greenWire.SetActive(true);
        }
        Debug.Log("Door is opened");
    
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
