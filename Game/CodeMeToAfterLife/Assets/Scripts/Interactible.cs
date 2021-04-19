using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
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
            }
        }
    }
    
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);  
    }
    
    private void OnTriggerEnter3D(SphereCollider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player now in range");
        }
    }
    
    private void OnTriggerExit3D(SphereCollider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player now is no longer in range");
        }
    }
    

}
