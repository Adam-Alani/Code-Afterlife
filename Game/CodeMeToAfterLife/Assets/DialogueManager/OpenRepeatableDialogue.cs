﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRepeatableDialogue : MonoBehaviour
{
    public KeyCode openKey = KeyCode.G;
    public Dialogue dialogue;
    public Dialogue repeatableDialogue;

    private bool isInRange;
    private bool isOpen;
    public bool displayRepeatableDialogue;

    void Update()
    {
        if (isInRange) // if we're in range to interact
        {
            if (Input.GetKeyDown(openKey)) // if the right key is pressed
            {
                if (!isOpen && !displayRepeatableDialogue)
                {
                    isOpen = true;
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                    isOpen = false;
                    displayRepeatableDialogue = true;
                }
                else if (displayRepeatableDialogue)
                {
                    isOpen = true;
                    FindObjectOfType<DialogueManager>().StartDialogue(repeatableDialogue);
                    isOpen = false;
                }
            }
        }
    }
    
    // method called by unity when there is a collision
    // this methods requires a collider on each object that should interact, with one that has the option "is trigger" enabled, and at minimum one rigidbody on either of the objet
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerController>().PV.IsMine)
        {
            isInRange = true;
        }
    }
    
    // method called by unity when the collision ends
    // this methods requires a collider on each object that should interact, with one that has the option "is trigger" enabled, and at minimum one rigidbody on either of the objet
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerController>().PV.IsMine)
        {
            isInRange = false;
        }
    }
}