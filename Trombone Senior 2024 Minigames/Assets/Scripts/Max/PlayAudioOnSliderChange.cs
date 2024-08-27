using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnSliderChange : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource musicSource;

    public void PlaySource(bool sound)
    {
        if (sound) soundSource.Play(); 
        else musicSource.Play(); 
    }
}
