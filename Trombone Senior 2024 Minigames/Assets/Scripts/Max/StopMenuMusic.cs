using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMenuMusic : MonoBehaviour
{
    [SerializeField] float fadeTime;
    [SerializeField] float interval;
    [SerializeField] AudioSource shopSource;
    private void Start()
    {
        if (AudioManager.menuMusicInstance)
        {
            StartCoroutine(FadeMusic(AudioManager.menuMusicInstance, true));
        }

    }

    IEnumerator FadeMusic(AudioSource source, bool destroy = false)
    {
        float t = 0f;
        while (t < fadeTime) 
        {
            yield return new WaitForSeconds(interval);
            t += interval;
            source.volume = Mathf.Lerp(1f, 0f, t / fadeTime);
        }

        if (destroy) Destroy(source.gameObject);
    }

    public void FadeShopMusic() => StartCoroutine(FadeMusic(shopSource));
}
