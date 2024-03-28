using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Cinemachine;
using System.Collections;

public class MaxGameManager : MonoBehaviour
{
    [SerializeField] MaxCharacterController controller;
    [SerializeField] List<SpriteRenderer> sprites;
    [SerializeField] GameObject startPanel;
    [SerializeField] List<GameObject> startObjects;
    [SerializeField] List<AnimatorSetTrigger> animators;
    [SerializeField] bool startedInitial;
    public static bool started;
    public static bool gameOver;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] float shrinkRate;
    [SerializeField] float startDelay;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float canvasAppearTime;
    private float accumulated = 0;
    void Start()
    {
        started = false;
        gameOver = false;
        foreach (SpriteRenderer sprite in sprites) sprite.enabled = false;
        foreach (var v in startObjects) v.SetActive(false);
        canvasGroup.alpha = 0;
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
            if (accumulated < canvasAppearTime)
            {
                accumulated += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0, 1, accumulated / canvasAppearTime);
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
        var activeTouches = Touch.activeTouches;

        if (activeTouches.Count > 0)
        {
            startedInitial = true;
            startPanel.SetActive(false);
        }
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
}
