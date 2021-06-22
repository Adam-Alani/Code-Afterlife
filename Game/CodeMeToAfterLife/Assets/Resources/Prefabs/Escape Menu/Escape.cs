using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Escape : MonoBehaviour
{

    public TerminalBehavior[] Terminals; // All the terminals in the current scene
    public GameObject MenuCanvas; // The canvas menu...

    public bool isOpen; // if the menu canvas is visible or not
    public KeyCode openKey;
    public KeyCode closeKey;
    public bool openning; // if we're openning the menu
    public PhotonView PV;

    public Slider VolumeSlider; 

    private bool ClosingTerm;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTerminals();
        GetVolume();
    }

    /// <summary>
    /// Find the terminals in the current scene
    /// </summary>
    void UpdateTerminals()
    {
        Terminals = FindObjectsOfType<TerminalBehavior>();
        Debug.Log("Escape menu : Updated Terminals");
    }
    
    // Update is called once per frame
    void Update()
    {
        OpenMe();
        GetVolume(); // not sure --------------------------------------------------------------------------
    }

    /// <summary>
    /// Try to open / close the escape menu
    /// </summary>
    void OpenMe()
    {
        if (!Input.GetKeyDown(openKey))
        {
            openning = false;
            ClosingTerm = false;
        }

        if(ClosingTerm)
            return;

        foreach(TerminalBehavior terminal in Terminals)
        {
            if (terminal.isOpen)
            {
                ClosingTerm = true;
                return;        
            }
        }
        
        if (Input.GetKeyDown(openKey)) // if the right key is pressed
        {
            if (!isOpen && PV.IsMine)
            {
                OpenMenu();
            }else 
                Debug.Log("Escape Menu : already opened");
        }

        if (Input.GetKeyDown(closeKey) && !openning)
        {
            if (isOpen && PV.IsMine)
            {
                CloseMenu();
            }
        }
    }

    /// <summary>
    /// Open the menu
    /// </summary>
    void OpenMenu()
    {
        Debug.Log("Escape Menu : is opened");
        MenuCanvas.SetActive(true);
        isOpen = true;
        openning = true;
    }

    /// <summary>
    /// Close the menu
    /// </summary>
    public void CloseMenu()
    {
        Debug.Log("Escape Menu : is closed");
        isOpen = false;
        MenuCanvas.SetActive(false);
    }

    /// <summary>
    /// Leave the room
    /// </summary>
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
        //Destroy(gameObject);
    }
    
    /// <summary>
    /// switch between fullscreen and windowed
    /// </summary>
    public void SetFullscreen (bool fullscreen) 
    {
        Screen.fullScreen = fullscreen;
        Debug.Log($"Escape Menu : Set Fullscreen active : {fullscreen}");
    }

    /// <summary>
    /// Changes the volume
    /// </summary>
    public void SetVolume(float volume)
    {
        AudioManager audiomanager = FindObjectOfType<AudioManager>();
        audiomanager.SetVolume(volume);
    }

    /// <summary>
    /// Gets the volume
    /// </summary>
    void GetVolume()
    {
        AudioManager audiomanager = FindObjectOfType<AudioManager>();
        VolumeSlider.value = audiomanager.GetVolume();
    }

}
