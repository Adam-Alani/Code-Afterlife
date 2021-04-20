using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalBehavior : MonoBehaviour
{
    public bool isOpen;
    public bool isInRange;
    public KeyCode openKey;
    public KeyCode closeKey;
    // public Animator animator; // if we want to add an animation
    
    public Scene currentScene;

    void Start()
    {
        
    }

    void Update()
    {
        if (isInRange) // if we're in range to interact
        {
            if (Input.GetKeyDown(openKey)) // if the right key is pressed
            {
                if (!isOpen)
                {
                    isOpen = true;
                    OpenTerminal();
                }
            }
            if (Input.GetKeyDown(closeKey))
            {
                if (isOpen)
                {
                    isOpen = false;
                    CloseTerminal();
                }
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
    
    public void OpenTerminal()
    {
        SceneManager.LoadScene("Assets/CodeEditor/Code Editor.unity", LoadSceneMode.Additive);
        Debug.Log("Terminal is open");
            
        // animator.SetBool("IsOpen", isOpen); // if we want to add an animation
    }

    public void CloseTerminal()
    {
        // SceneManager.LoadScene("Assets/Scenes/Test Terminal.unity");
        SceneManager.UnloadSceneAsync("Assets/CodeEditor/Code Editor.unity");
        Debug.Log("Terminal is close");
    }
}
