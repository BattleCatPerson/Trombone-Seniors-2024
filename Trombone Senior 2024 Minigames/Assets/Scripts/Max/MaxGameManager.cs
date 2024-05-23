using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using System.Collections;
using UnityEngine.EventSystems;

public class MaxGameManager : MonoBehaviour
{
    public static MaxGameManager instance;
    [Header("Enabling")]
    [SerializeField] MaxCharacterController controller;
    [SerializeField] List<SpriteRenderer> sprites;
    [SerializeField] GameObject startPanel;
    [SerializeField] List<GameObject> startObjects;
    [SerializeField] List<AnimatorSetTrigger> animators;
    public bool startedInitial;
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
    public bool tutorialActive;
    [Header("Pause")]
    [SerializeField] GameObject pauseMenu;
    public bool pauseActive;
    [Header("Restart")]
    [SerializeField] List<Transform> movingObjects;
    [SerializeField] bool restarting;
    [SerializeField] PoliceCarFollow policeCar;
    [SerializeField] MapGeneration mapGeneration;
    [SerializeField] CinemachineBrain brain;
    [SerializeField] float restartCameraTime;
    [SerializeField] float defaultTransitionTime;
    public UnityEvent restartEvent;
    public Dictionary<Transform, Vector3> initialPositions;
    [Header("Shield Active")]
    [SerializeField] AnimatorSetTrigger shieldAnimator;

    private float accumulated = 0;
    private void Awake()
    {
        instance = this;
        restartEvent = new();
    }
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

        restartEvent.AddListener(ResetGame);
        defaultTransitionTime = brain.m_DefaultBlend.m_Time;
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
                PlayTutorial();
                PlayerPrefs.SetInt("Played Tutorial", 1);
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
            brain.m_DefaultBlend.m_Time = defaultTransitionTime;
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

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseActive = true;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        pauseActive = false;
    }

    public void EnablePanel() => tutorialAnimator.SetTrigger("Rise");
    public void DisablePanel()
    {
        tutorialActive = false;
        tutorialAnimator.SetTrigger("Fall");
        if (!pauseMenu.active) UnpauseGame();
    }
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
        restartEvent?.Invoke();
        //policeCar.Disable();
        //controller.ResetGame();
        //reset drones
    }

    public void ResetGame()
    {
        brain.m_DefaultBlend.m_Time = restartCameraTime;
        cam.m_Lens.OrthographicSize = camSizeInitial;
        cam.Priority = 11;
        gameCam.Follow = null;
        restarting = true;
        started = false;
        gameOver = false;
        startedInitial = false;
        startDelay = startDelayInitial;
        canvasGroup.alpha = 0;
        foreach (Transform t in initialPositions.Keys) t.position = initialPositions[t];
        accumulated = 0;

        foreach (SpriteRenderer sprite in sprites) sprite.enabled = false;
        foreach (var v in startObjects) v.SetActive(false);
        foreach (AnimatorSetTrigger a in animators) a.ResetTrigger();
    }

    public void ShieldActive(bool active)
    {
        if (active) shieldAnimator.SetTrigger();
        else shieldAnimator.ResetTrigger();
    }

    public void PlayTutorial()
    {
        tutorialActive = true;
        PauseGame();
        EnablePanel();
    }
}
