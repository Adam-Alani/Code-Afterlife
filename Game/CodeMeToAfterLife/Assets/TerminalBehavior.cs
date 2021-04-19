using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalBehavior : MonoBehaviour
{
    public bool isOpen;
    public Animator animator; // if we want to add an animation

    public void OpenTerminal()
    {
        if (!isOpen)
        {
            // do stuff 
            isOpen = true;
            Debug.Log("Terminal is open");
            animator.SetBool("IsOpen", isOpen);
        }
    }
}
