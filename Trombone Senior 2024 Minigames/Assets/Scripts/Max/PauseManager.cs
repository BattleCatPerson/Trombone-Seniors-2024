using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] float currentTimeScale;
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (Time.timeScale > 0) currentTimeScale = Time.timeScale;
            else currentTimeScale = 1;
            Time.timeScale = 0f;
        }
        if (!pause)
        {
            if (MaxGameManager.instance)
            {
                if (MaxGameManager.instance.tutorialActive || MaxGameManager.instance.pauseActive) return;
                if (MaxGameManager.instance.startedInitial && !MaxGameManager.instance.pauseActive) MaxGameManager.instance.EnablePauseMenu();
                else Time.timeScale = currentTimeScale;
            }
            else
            {
                Time.timeScale = currentTimeScale;
            }
        }
    }
}
