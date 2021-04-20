using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class interactible : MonoBehaviour
{
    public float radius = 3f;

    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;

    void Start()
    {
        
    }

    void Update()
    {
        if (isInRange) // if we're in range to interact
        {
            if (Input.GetKeyDown(interactKey)) // if the right key is pressed
            {
                interactAction.Invoke(); // fire event
                Debug.Log("Event fired");
            }
        }
    }
    
    // draw a sphere on the editor around the interactible object, purely for debuging
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);  
    }
    
    
    // method called by unity when there is a collision
    // this methods requires a collider on each object that should interact, with one that has the option "is trigger" enabled, and at minimum one rigidbody on either of the objet
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player now in range");
        }
    }
    
    // method called by unity when the collision ends
    // this methods requires a collider on each object that should interact, with one that has the option "is trigger" enabled, and at minimum one rigidbody on either of the objet
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player now is no longer in range");
        }
    }
}
