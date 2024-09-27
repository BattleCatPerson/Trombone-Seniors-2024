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
        sourcesStatic.Clear();
        sourcesStatic.AddRange(sources);
    }
    void Update()
    {
        foreach (AudioSource source in sourcesStatic) 
        {
            source.volume = MaxGameManager.gameOver ? 0 : 1;
        }
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

    public void StopGame()
    {
        StopAllSounds();
        startupPlaying = false;
    }
}
