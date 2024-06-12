using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private void OnApplicationPause(bool pause)
    {
        if (pause) Time.timeScale = 0f;
        if (!pause)
        {
            if (MaxGameManager.instance)
            {
                if (MaxGameManager.instance.tutorialActive || MaxGameManager.instance.pauseActive) return;

                if (MaxGameManager.instance.startedInitial && !MaxGameManager.instance.pauseActive) MaxGameManager.instance.EnablePauseMenu();
                else Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}
