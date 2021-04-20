using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalBehavior : MonoBehaviour
{
    public bool isOpen;
    // public Animator animator; // if we want to add an animation
    
    private Scene currentScene;

	// OpenTerminal() is the function that we fire with the interactible script
    public void OpenTerminal()
    {
        if (!isOpen)
        {
            isOpen = true;
            currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene("Assets/CodeEditor/Code Editor.unity");
            Debug.Log("Terminal is open");
            
            // animator.SetBool("IsOpen", isOpen); // if we want to add an animation
        }
    }
    
    // CloseTerminal() is called on when we close the terminal from the codeEditor
    public void CloseTerminal()
    {
        if (isOpen)
        {
            isOpen = false;
            Debug.Log("Terminal is closed");
            SceneManager.LoadScene(currentScene.path);
        }
    }
    
    
}
