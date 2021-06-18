using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class Escape : MonoBehaviour
{

    public TerminalBehavior[] Terminals; // All the terminals in the current scene
    public GameObject MenuCanvas; // The canvas menu...

    public bool isOpen; // if the menu canvas is visible or not
    public KeyCode openKey;
    public KeyCode closeKey;
    public bool openning; // if we're openning the menu

    /// <summary>
    /// Setup of the object
    /// </summary>
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// When we change scene
    /// </summary>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //DontDestroyOnLoad(gameObject);

        UpdateTerminals();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTerminals();
    }

    // Update is called once per frame
    void Update()
    {
        OpenMe();
    }


    /// <summary>
    /// Find the terminals in the current scene
    /// </summary>
    void UpdateTerminals()
    {
        Terminals = FindObjectsOfType<TerminalBehavior>();
        Debug.Log("Updated Terminals");
    }

    /// <summary>
    /// Try to open / close the escape menu
    /// </summary>
    void OpenMe()
    {
        if (!Input.GetKeyDown(openKey))
            openning = false;


        foreach(TerminalBehavior terminal in Terminals)
        {
            if (terminal.isOpen)
                return;
        }
        
        if (Input.GetKeyDown(openKey)) // if the right key is pressed
        {
            if (!isOpen)
            {
                OpenMenu();
            }else 
                Debug.Log("Escape Menu already opened");
        }

        if (Input.GetKeyDown(closeKey) && !openning)
        {
            if (isOpen)
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
        Debug.Log("Escape Menu is open");
        MenuCanvas.SetActive(true);
        isOpen = true;
        openning = true;
    }

    /// <summary>
    /// Close the menu
    /// </summary>
    public void CloseMenu()
    {
        Debug.Log("Escape Menu is close");
        isOpen = false;
        MenuCanvas.SetActive(false);
    }

    /// <summary>
    /// Leave the room
    /// </summary>
    public void Leave()
    {
        /*if()
        
        PhotonPlayer[] players = FindObjectsOfType<PhotonPlayer>();*/
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
    

    /// <summary>
    /// switch between fullscreen and windowed
    /// </summary>
    public void SetFullscreen (bool fullscreen) 
    {
        Screen.fullScreen = fullscreen;
        Debug.Log($"Set Fullscreen active : {fullscreen}");
    }

    /// <summary>
    /// Changes the volume
    /// </summary>
    public void SetVolume(float volume)
    {
        AudioManager audiomanager = FindObjectOfType<AudioManager>();
        audiomanager.SetVolume(volume);
    }
}
