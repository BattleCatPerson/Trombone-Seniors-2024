using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartAndQuit : MonoBehaviour
{
    [SerializeField] string restartScene;
    [SerializeField] string quitScene;
    [SerializeField] string otherScene;

    public void Restart() => SceneManager.LoadScene(restartScene);
    public void Quit() => SceneManager.LoadScene(quitScene);
    public void LoadOther() => SceneManager.LoadScene(otherScene);
    public void DisableMenuMusicReset()
    {
        AudioManager.playOnStart = false;
    }
    public void EnableMenuMusicReset()
    {
        AudioManager.playOnStart = true;
    }

    public void ChangeScene(string s) => restartScene = s;
}
