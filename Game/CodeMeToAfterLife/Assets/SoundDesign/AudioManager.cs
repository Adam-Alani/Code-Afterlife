using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
   public Sound[] sounds;
   public static AudioManager instance;
   
   void Awake()
   {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
       
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
       
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            Debug.Log("Sound prepared " + s.name);
        }
   
   }
   
   void Start ()
   {
	    Play("MainMusic");
   }
   
   public void Play (string name)
   {
   		Sound s = Array.Find(sounds, sound => sound.name == name);
   		if (s == null)
   		{
   			Debug.LogWarning("Sound: " + name + " not found!");
   			return;
   		}
   		s.source.Play();
        Debug.Log($"Playing {name}");
   }

   public void SetVolume(float volume)
   {
       Debug.Log($"Setting volume to {volume}");
       foreach(Sound sound in sounds)
       {
           Debug.Log($"Set the volume of {sound.name} to {volume}");
           sound.SetVolume(volume);
       }
   }

   public float GetVolume()
   {
       //Debug.Log($"Getting volume");
       return sounds[0].volume;
   }
}
