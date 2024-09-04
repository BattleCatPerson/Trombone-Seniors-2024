using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarAudio : MonoBehaviour
{
    [SerializeField] AudioSource startupSource;
    [SerializeField] AudioSource loopSource;
    [SerializeField] AudioSource sirenSource;
    [SerializeField] List<AudioSource> sources;
    [SerializeField] bool startupPlaying;
    public static List<AudioSource> sourcesStatic = new();
    private void Awake()
    {
        sourcesStatic.AddRange(sources);
        MaxGameManager.instance.restartEvent.AddListener(StopAllSounds);
    }
    void Update()
    {
        if (startupPlaying && !startupSource.isPlaying)
        {
            loopSource.Play();
            startupPlaying = false;
        }
        foreach (var s in PoliceCarAudio.sourcesStatic) s.pitch = Time.timeScale;
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
    public void PauseSounds(bool paused)
    {
        if (paused)
        {
            startupSource.Pause();
            loopSource.Pause();
            sirenSource.Pause();
        }
        else
        {
            startupSource.Play();
            loopSource.Pause();
            sirenSource.Pause();
        }
    }
}
