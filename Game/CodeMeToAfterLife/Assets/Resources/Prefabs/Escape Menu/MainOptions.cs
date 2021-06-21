using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MainOptions : MonoBehaviour
{
    public Slider VolumeSlider; 

    /// <summary>
    /// Setup of the object
    /// </summary>
    void Awake()
    {
        GetVolume();
    }

    /// <summary>
    /// When we change scene
    /// </summary>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetVolume();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetVolume();
    }


    void OnEnable()
    {
        GetVolume();
    }

   
    /// <summary>
    /// switch between fullscreen and windowed
    /// </summary>
    public void SetFullscreen (bool fullscreen) 
    {
        Screen.fullScreen = fullscreen;
        Debug.Log($"Option Menu : Set Fullscreen active : {fullscreen}");
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
