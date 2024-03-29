using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.EventSystems;

public class MaxGameManager : MonoBehaviour
{
    [Header("Enabling")]
    [SerializeField] MaxCharacterController controller;
    [SerializeField] List<SpriteRenderer> sprites;
    [SerializeField] GameObject startPanel;
    [SerializeField] List<GameObject> startObjects;
    [SerializeField] List<AnimatorSetTrigger> animators;
    [SerializeField] bool startedInitial;
    [SerializeField] 
    public static bool started;
    public static bool gameOver;
    public static bool autoStart = false;
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] float shrinkRate;
    [SerializeField] float startDelay;
    [Header("UI")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float canvasAppearTime;
    [Header("Tutorial")]
    [SerializeField] bool playedTutorial;
    [SerializeField] float tutorialPopupDelay;
    [SerializeField] Animator tutorialAnimator;
    private float accumulated = 0;
    void Start()
    {
        started = false;
        gameOver = false;
        foreach (SpriteRenderer sprite in sprites) sprite.enabled = false;
        foreach (var v in startObjects) v.SetActive(false);
        canvasGroup.alpha = 0;
        if (!PlayerPrefs.HasKey("Played Tutorial") || PlayerPrefs.GetInt("Played Tutorial") == 0) PlayerPrefs.SetInt("Played Tutorial", 0);
        else playedTutorial = true;

        if (autoStart)
        {
            StartGameInitial();
            autoStart = false;
        }
    }

    void Update()
    {
        if (gameOver)
        {
            foreach (var v in startObjects) v.SetActive(false);
            canvasGroup.alpha = 0;
            return;
        }
        if (started)
        {
            accumulated += Time.deltaTime;
            if (accumulated < canvasAppearTime) canvasGroup.alpha = Mathf.Lerp(0, 1, accumulated / canvasAppearTime);
            else canvasGroup.alpha = 1;
            if (accumulated >= tutorialPopupDelay && playedTutorial == false)
            {
                playedTutorial = true;
                PlayerPrefs.SetInt("Played Tutorial", 1);
                //go tutorial panel
                PauseGame();
                EnablePanel();
            }
            return;
        }
        if (startedInitial)
        {
            startDelay -= Time.deltaTime;
            cam.m_Lens.OrthographicSize -= shrinkRate * Time.deltaTime;

            if (startDelay <= 0) StartGame();
            return;
        }
    }
    public void StartGameInitial()
    {
        startedInitial = true;
        startPanel.SetActive(false);
    }
    public void StartGame()
    {
        started = true;
        foreach (SpriteRenderer sprite in sprites) sprite.enabled = true;
        foreach (AnimatorSetTrigger a in animators) a.SetTrigger();
        controller.StartGame();
        cam.Priority = 0;
        foreach (var v in startObjects) v.SetActive(true);
    }

    public void PauseGame() => Time.timeScale = 0f;
    public void UnpauseGame() => Time.timeScale = 1f;

    public void EnablePanel() => tutorialAnimator.SetTrigger("Rise");
    public void DisablePanel() => tutorialAnimator.SetTrigger("Fall");
    public void SetAutoStart() => autoStart = true;
}
