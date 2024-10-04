using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    [Header("Audio")]
    [SerializeField] AudioManager audioManager;
    [Header("Pause")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] float currentTimeScale;
    public bool pauseActive;
    [Header("Restart")]
    [SerializeField] List<Transform> movingObjects;
    [SerializeField] bool restarting;
    [SerializeField] PoliceCarFollow policeCar;
    [SerializeField] PoliceCarAudio policeCarAudio;
    [SerializeField] MapGeneration mapGeneration;
    [SerializeField] CinemachineBrain brain;
    [SerializeField] float restartCameraTime;
    [SerializeField] float defaultTransitionTime;
    public UnityEvent restartEvent;
    public Dictionary<Transform, Vector3> initialPositions;
    [Header("Shield Active")]
    [SerializeField] AnimatorSetTrigger shieldAnimator;
    [SerializeField] Shield shield;
    [Header("Outline")]
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material outlineMaterial;
    [SerializeField] List<SpriteRenderer> outlineSprites;
    [SerializeField] SpriteRenderer carOutlineSprite;
    [SerializeField] ProjectileSpawner projectileSpawner;
    [SerializeField] Toggle outlineToggle;
    [Header("Upgrades")]
    [SerializeField] LoadData loadData;
    public UnityEvent<List<CosmeticData.Upgrade>> loadEvent;
    private float accumulated = 0;
    [Header("Shield")]
    public bool shieldActive;
    private void Awake()
    {
        instance = this;
        restartEvent = new();
    }
    void Start()
    {
        Time.timeScale = 1;
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

        if (!PlayerPrefs.HasKey("Outline")) PlayerPrefs.SetInt("Outline", 0);
        EnableOutlines();
        //menuSource.PlayDelayed(introClip.length - 0.25f);
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
        loadEvent?.Invoke(loadData.Data.upgrades);
        startedInitial = true;
        audioManager.SwitchToMainGameMusic();
    }
    public void StartGame()
    {
        started = true;
        foreach (SpriteRenderer sprite in sprites) sprite.enabled = true;
        foreach (AnimatorSetTrigger a in animators) a.SetTrigger();
        controller.StartGame();
        cam.Priority = 0;
        foreach (var v in startObjects) v.SetActive(true);
        audioManager.StartGameMusic();
        audioManager.Explode();
    }

    public void PauseGame()
    {
        currentTimeScale = Time.timeScale;
        if (Time.timeScale == 0) currentTimeScale = 1;

        Time.timeScale = 0f;
        pauseActive = true;
        foreach (var s in PoliceCarAudio.sourcesStatic) s.pitch = Time.timeScale;
    }
    public void UnpauseGame()
    {
        Time.timeScale = currentTimeScale;
        pauseActive = false;
        audioManager.EnablePauseMusic(false);
        foreach (var s in PoliceCarAudio.sourcesStatic) s.pitch = Time.timeScale;
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
        audioManager.EnablePauseMusic(true);
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
        audioManager.PlayGameOver(false);
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
        if (active && !shield.Shooting && !shieldActive)
        {
            shieldAnimator.SetTrigger();
            shieldActive = true;
        }
        else if (!shield.Shooting && shieldActive)
        {
            shieldActive = false;
            shieldAnimator.ResetTrigger();
        }
    }

    public void PlayTutorial(bool inGame = true)
    {
        tutorialActive = true;
        if (inGame) PauseGame();
        EnablePanel();
    }

    public void ToggleOutlines()
    {
        bool on = PlayerPrefs.GetInt("Outline") == 1;
        if (on) PlayerPrefs.SetInt("Outline", 0);
        else PlayerPrefs.SetInt("Outline", 1);
        EnableOutlines();
    }
    public void EnableOutlines()
    {
        bool on = PlayerPrefs.GetInt("Outline") == 1;
        if (!on)
        {
            foreach (var v in outlineSprites) v.material = defaultMaterial;
            carOutlineSprite.material = defaultMaterial;
            projectileSpawner.outline = false;
            policeCar.outline = false;
            outlineToggle.isOn = false;
        }
        else
        {
            foreach (var v in outlineSprites) v.material = outlineMaterial;
            if (policeCar.CurrentCar.outline) carOutlineSprite.material = outlineMaterial;
            projectileSpawner.outline = true;
            policeCar.outline = true;
            outlineToggle.isOn = true;
        }
    }
    public void GameOver()
    {
        gameOver = true;
        audioManager.DisableMusic();
        policeCarAudio.StopGame();
    }
}
