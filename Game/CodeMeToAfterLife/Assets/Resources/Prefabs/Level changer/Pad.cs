using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{

    public bool Is_in;

    // Start is called before the first frame update
    void Start()
    {
        Is_in = false;
    }

    // method called by unity when the collision starts
    // 
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Is_in = true;
            Debug.Log("Player now in range");
        }
    }
    
    // method called by unity when the collision ends
    // 
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Is_in = false;
            Debug.Log("Player now is no longer in range");
        }
    }
}
