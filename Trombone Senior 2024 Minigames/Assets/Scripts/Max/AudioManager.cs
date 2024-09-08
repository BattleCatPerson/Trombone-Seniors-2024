using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("Menu Music")]
    [SerializeField] AudioClip introClip;
    [SerializeField] AudioClip menuLoop;
    [SerializeField] AudioMixerGroup mixerGroup;
    public static AudioSource menuMusicInstance;
    [Header("Pause Music")]
    [SerializeField] AudioSource pauseMusic;
    public static bool playOnStart;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        if (menuMusicInstance == null)
        {
            menuMusicInstance = new GameObject().AddComponent<AudioSource>();
            menuMusicInstance.gameObject.name = "Menu Music";
            menuMusicInstance.playOnAwake = false;
            menuMusicInstance.loop = true;
            menuMusicInstance.outputAudioMixerGroup = mixerGroup;
            DontDestroyOnLoad(menuMusicInstance);
            InitializeMenuMusic();
        }
        else
        {
            menuMusicInstance.volume = 1;
            if (playOnStart)
            {
                menuMusicInstance.Stop();
                InitializeMenuMusic();
            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializeMenuMusic()
    {
        menuMusicInstance.PlayOneShot(introClip);
        menuMusicInstance.clip = menuLoop;
        menuMusicInstance.PlayScheduled(AudioSettings.dspTime + introClip.length);
    }

    public void EnablePauseMusic(bool enable)
    {
        if (enable) pauseMusic.Play();
        else pauseMusic.Stop();
    }

    public IEnumerator FadeMusic(AudioSource s, float interval, float duration)
    {
        float accumulated = 0f;
        while (accumulated < duration)
        {
            yield return new WaitForSeconds(interval);
            accumulated += interval;
            s.volume = (Mathf.Lerp(1, 0, accumulated / duration));
        }
    }

    public void SwitchToMainGameMusic()
    {
        if (menuMusicInstance.volume > 0) StartCoroutine(FadeMusic(menuMusicInstance, 0.05f, 2f));
    }
}
