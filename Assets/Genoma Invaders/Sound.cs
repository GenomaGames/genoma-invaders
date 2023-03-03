using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public AudioClip Clip
    {
        set
        {
            audioSource.clip = value;
        } 
    }

    [HideInInspector]
    public AudioSource audioSource;

    public void Play()
    {
        audioSource.Play();
    }
}
