using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private void OnApplicationPause(bool pause)
    {
        if (pause) Time.timeScale = 0f;
        if (!pause) Time.timeScale = 1f;
    }
}
