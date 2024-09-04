using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarAudio : MonoBehaviour
{
    [SerializeField] AudioSource startupSource;
    [SerializeField] AudioSource loopSource;
    [SerializeField] AudioSource sirenSource;
    [SerializeField] bool startupPlaying;
    void Update()
    {
        if (startupPlaying && !startupSource.isPlaying)
        {
            loopSource.Play();
        }
    }

    public void PlayStartUp()
    {
        startupPlaying = true;
        startupSource.Play();
    }
    public void PlaySiren() => sirenSource.Play();
    public void StopAllSounds()
    {
        startupSource.Stop();
        loopSource.Stop();
        sirenSource.Stop();
    }
}
