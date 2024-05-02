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
    [SerializeField] CinemachineVirtualCamera gameCam;
    [SerializeField] Transform mainCam;
    float camSizeInitial;
    [SerializeField] float shrinkRate;
    [SerializeField] float startDelay;
    float startDelayInitial;
    [Header("UI")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float canvasAppearTime;
    [Header("Tutorial")]
    [SerializeField] bool playedTutorial;
    [SerializeField] float tutorialPopupDelay;
    [SerializeField] Animator tutorialAnimator;
    [Header("Pause")]
    [SerializeField] GameObject pauseMenu;
    [Header("Restart")]
    [SerializeField] List<Transform> movingObjects;
    [SerializeField] bool restarting;
    [SerializeField] PoliceCarFollow policeCar;
    [SerializeField] MapGeneration mapGeneration;
    public Dictionary<Transform, Vector3> initialPositions;

    private float accumulated = 0;
    void Start()
    {
        initialPositions = new();
        started = false;
        gameOver = false;
        startDelayInitial = startDelay;
        foreach (SpriteRenderer sprite in sprites) sprite.enabled = false;
        foreach (var v in startObjects) v.SetActive(false);
        foreach (Transform t in movingObjects) initialPositions.Add(t, t.position);
        canvasGroup.alpha = 0;
        if (!PlayerPrefs.HasKey("Played Tutorial") || PlayerPrefs.GetInt("Played Tutorial") == 0) PlayerPrefs.SetInt("Played Tutorial", 0);
        else playedTutorial = true;

        if (autoStart)
        {
            startPanel.SetActive(false);
            StartGameInitial();
            autoStart = false;
        }
        camSizeInitial = cam.m_Lens.OrthographicSize;
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
        if (restarting && Vector3.Distance(mainCam.position, cam.transform.position) < 0.01f)
        {
            restarting = false;
            gameCam.Follow = controller.transform;
            mapGeneration.ResetPermaFloor();
            StartGameInitial();
        }
    }
    public void StartGameInitial()
    {
        startedInitial = true;
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
    public void EnablePauseMenu()
    {
        pauseMenu.SetActive(true);
        PauseGame();
    }

    public void DisablePauseMenu()
    {
        pauseMenu.SetActive(false);
        UnpauseGame();
    }

    public void Restart()
    {
        cam.m_Lens.OrthographicSize = camSizeInitial;
        cam.Priority = 11;
        gameCam.Follow = null;
        restarting = true;
        started = false;
        gameOver = false;
        startedInitial = false;
        startDelay = startDelayInitial;
        policeCar.Disable();
        accumulated = 0;

        controller.ResetGame();
        foreach (SpriteRenderer sprite in sprites) sprite.enabled = false;
        foreach (var v in startObjects) v.SetActive(false);
        foreach (AnimatorSetTrigger a in animators) a.ResetTrigger();

        canvasGroup.alpha = 0;

        foreach (Transform t in initialPositions.Keys) t.position = initialPositions[t];
    }
}
