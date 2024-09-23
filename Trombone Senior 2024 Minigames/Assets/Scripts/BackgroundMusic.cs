using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static GameObject audio;
    [SerializeField] GameObject audioLocal;
    void Start()
    {
        if (audio == null) audio = Instantiate(audioLocal);
        DontDestroyOnLoad(audio);
    }

    public void DestroyAudio()
    {
        if (audio) Destroy(audio);
    }
}
