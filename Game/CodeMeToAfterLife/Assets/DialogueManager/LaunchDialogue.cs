﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchDialogue : MonoBehaviour
{ 
    public Dialogue dialogue;
    private bool isInRange;
    private bool isLaunched;

    void Update()
    {
        if (isInRange) // if we're in range to interact
        {
            if (!isLaunched)
            {
                isLaunched = true;
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }
        }
    }
    
    // method called by unity when there is a collision
    // this methods requires a collider on each object that should interact, with one that has the option "is trigger" enabled, and at minimum one rigidbody on either of the objet
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }
    
    // method called by unity when the collision ends
    // this methods requires a collider on each object that should interact, with one that has the option "is trigger" enabled, and at minimum one rigidbody on either of the objet
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
