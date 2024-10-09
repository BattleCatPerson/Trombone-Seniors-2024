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
    [SerializeField] bool createMenuInstance = true;
    public static AudioSource menuMusicInstance;
    [SerializeField] AudioSource gameMusic;
    [Header("Alternate Track")]
    [SerializeField] AudioClip introAlternate;
    [SerializeField] AudioClip loopAlternate;
    [SerializeField] bool alternate;
    [SerializeField] GameObject toggleButton;
    [SerializeField] LoadData data;
    [Header("Pause Music")]
    [SerializeField] AudioSource pauseMusic;
    public static bool playOnStart;
    [Header("Game Music")]
    [SerializeField] bool gameMusicPlaying;
    [Header("Record Scratch")]
    [SerializeField] AudioClip scratch;
    [SerializeField] AudioSource scratchSource;
    [Header("Button Sound")]
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioSource buttonSource;
    [Header("Explosion")]
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioSource explosionSource;
    [Header("Game Over")]
    [SerializeField] AudioSource gameOverSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("Menu Music")) PlayerPrefs.SetInt("Menu Music", 0);
        int i = PlayerPrefs.GetInt("Menu Music");
        alternate = i == 1;
        if (createMenuInstance)
        {
            if (menuMusicInstance == null)
            {
                menuMusicInstance = new GameObject().AddComponent<AudioSource>();
                menuMusicInstance.gameObject.name = "Menu Music";
                menuMusicInstance.playOnAwake = false;
                menuMusicInstance.loop = true;
                menuMusicInstance.outputAudioMixerGroup = mixerGroup;
                DontDestroyOnLoad(menuMusicInstance);
                if (i == 0) InitializeMenuMusic();
                else if (i == 1) InitializeAlternateMenuMusic();
            }
            else
            {
                menuMusicInstance.volume = 1;
                if (playOnStart)
                {
                    menuMusicInstance.Stop();
                    if (i == 0) InitializeMenuMusic();
                    else if (i == 1) InitializeAlternateMenuMusic();
                }
            }
        }
        
        if (data) Upgrade(data.unlockedUpgrades);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMusicPlaying && !gameMusic.isPlaying)
        {
            StartGameMusic();
        }
    }
    public void InitializeMenuMusic()
    {
        menuMusicInstance.pitch = 1;
        menuMusicInstance.Stop();
        menuMusicInstance.PlayOneShot(introClip);
        menuMusicInstance.clip = menuLoop;
        menuMusicInstance.PlayScheduled(AudioSettings.dspTime + introClip.length);
    }
    public void InitializeAlternateMenuMusic()
    {
        menuMusicInstance.pitch = 1;
        menuMusicInstance.Stop();
        menuMusicInstance.PlayOneShot(introAlternate);
        menuMusicInstance.clip = loopAlternate;
        menuMusicInstance.PlayScheduled(AudioSettings.dspTime + introAlternate.length);
    }

    public void EnablePauseMusic(bool enable)
    {
        if (enable)
        {
            gameMusic.pitch = 0;
            menuMusicInstance.pitch = 0;
            pauseMusic.Play();
        }
        else
        {
            pauseMusic.Stop();
            gameMusic.pitch = 1;
            menuMusicInstance.pitch = 1;
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
            s.volume = (Mathf.Lerp(0, 1, accumulated / duration));
        }
        s.volume = 1;
    }
    

    public void SwitchToMainGameMusic()
    {
        if (menuMusicInstance.volume > 0)
        {
            menuMusicInstance.Stop();
        }


    }
    public void StartGameMusic()
    {
        gameMusic.volume = 0;
        StartCoroutine(FadeMusicIn(gameMusic, 0.05f, 2f));
        gameMusic.Play();
        gameMusicPlaying = true;
    }

    public void DisableMusic(float t = 1f)
    {
        StartCoroutine(FadeMusic(gameMusic, 0.05f, t));
        gameMusicPlaying = false;
    }
    public void SwitchTracks()
    {
        if (!alternate) InitializeAlternateMenuMusic();
        else InitializeMenuMusic();
        alternate = !alternate;
        int i = alternate ? 1 : 0;
        PlayerPrefs.SetInt("Menu Music", i);
    }
    public void Upgrade(List<CosmeticData.Upgrade> upgrades)
    {
        foreach (var u in upgrades)
        {
            int id = u.id;
            if (id == 8)
            {
                toggleButton.SetActive(true);
            }
        }
    }
    public void PlayButtonSound() => buttonSource.PlayOneShot(buttonSound);
    public void Explode() => explosionSource.PlayOneShot(explosion);
    public void PlayGameOver(bool b)
    {
        if (b) gameOverSource.Play();
        else gameOverSource.Stop();
    }
    public void PlayScratch() => scratchSource.PlayOneShot(scratch);
}
