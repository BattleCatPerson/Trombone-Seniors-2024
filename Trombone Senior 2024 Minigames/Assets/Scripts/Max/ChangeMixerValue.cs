using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChangeMixerValue : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer mixer;

    const string SFX_VOLUME = "SoundVolume";
    const string MUSIC_VOLUME = "MusicVolume";

    private void Start()
    {
        if (PlayerPrefs.HasKey(SFX_VOLUME))
        {
            float sV = PlayerPrefs.GetFloat(SFX_VOLUME);
            soundSlider.value = Mathf.Pow(10, sV / 20f);
        }
        else
        {
            PlayerPrefs.SetFloat(SFX_VOLUME, 0f);
            soundSlider.value = 1f;
        }
        if (PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            float sV = PlayerPrefs.GetFloat(MUSIC_VOLUME);
            musicSlider.value = Mathf.Pow(10, sV / 20f);
        }
        else
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, 0f);
            musicSlider.value = 1f;
        }
    }

    public void UpdateSFXVolume()
    {
        float v = Mathf.Log10(soundSlider.value) * 20f;
        mixer.SetFloat(SFX_VOLUME, v);
        PlayerPrefs.SetFloat(SFX_VOLUME, v);
    }
    public void UpdateMusicVolume()
    {
        float v = Mathf.Log10(musicSlider.value) * 20f;
        mixer.SetFloat(MUSIC_VOLUME, v);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, v);
    }

}
