using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalBehavior : MonoBehaviour
{
    public bool isOpen;
    public Animator animator; // if we want to add an animation

	// OpenTerminal() is the function that we fire with the interactible script
    public void OpenTerminal()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Terminal is open");
            animator.SetBool("IsOpen", isOpen); // if we want to add an animation
        }
    }
}
