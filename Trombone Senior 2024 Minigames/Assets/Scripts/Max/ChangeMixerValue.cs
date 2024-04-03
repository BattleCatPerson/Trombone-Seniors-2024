using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChangeMixerValue : MonoBehaviour
{
    [SerializeField] float maxValue;
    [SerializeField] float minValue;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer mixer;
    private void Start()
    {
        
    }
    void Update()
    {
        float sValue = soundSlider.value;
        float mValue = musicSlider.value;

        mixer.SetFloat("Sound", minValue + (maxValue - minValue) * sValue);
        mixer.SetFloat("Music", minValue + (maxValue - minValue) * mValue);
    }
}
