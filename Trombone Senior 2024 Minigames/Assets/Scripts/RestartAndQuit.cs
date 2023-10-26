using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartAndQuit : MonoBehaviour
{
    [SerializeField] string restartScene;
    [SerializeField] string quitScene;

    public void Restart() => SceneManager.LoadScene(restartScene);
    public void Quit() => SceneManager.LoadScene(quitScene);
}
