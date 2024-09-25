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
    [SerializeField] AudioSource gameMusic;
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
        if (enable)
        {
            gameMusic.pitch = 0;
            pauseMusic.Play();
        }
        else
        {
            pauseMusic.Stop();
            gameMusic.pitch = 1;
        }
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
        s.volume = 0;
    }
    public IEnumerator FadeMusicIn(AudioSource s, float interval, float duration)
    {
        float accumulated = 0f;
        while (accumulated < duration)
        {
            yield return new WaitForSeconds(interval);
            accumulated += interval;
            Debug.Log(accumulated);
            s.volume = (Mathf.Lerp(0, 1, accumulated / duration));
        }
        s.volume = 1;
    }
    

    public void SwitchToMainGameMusic()
    {
        if (menuMusicInstance.volume > 0) StartCoroutine(FadeMusic(menuMusicInstance, 0.05f, 2f));
    }
    public void StartGameMusic()
    {
        gameMusic.volume = 0;
        StartCoroutine(FadeMusicIn(gameMusic, 0.05f, 2f));
        gameMusic.Play();
    }

    public void DisableMusic(float t = 1f) => StartCoroutine(FadeMusic(gameMusic, 0.05f, t));


}
