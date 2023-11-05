using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBackgroundMusic : MonoBehaviour
{
    void Start()
    {
        if (BackgroundMusic.audio) Destroy(BackgroundMusic.audio);
    }
}
