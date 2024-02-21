using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Cinemachine;
using System.Collections;

public class MaxGameManager : MonoBehaviour
{
    [SerializeField] MaxCharacterController controller;
    [SerializeField] GameObject startPanel;
    [SerializeField] List<GameObject> startObjects;
    [SerializeField] List<AnimatorSetTrigger> animators;
    [SerializeField] bool startedInitial;
    public static bool started;
    public static bool gameOver;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] float camInitialSize;
    [SerializeField] float shrinkRate;
    [SerializeField] float startDelay;
    void Start()
    {
        cam.m_Lens.OrthographicSize = camInitialSize;
        started = false;
        gameOver = false;
        foreach (var v in startObjects) v.SetActive(false);
    }

    void Update()
    {
        if (started) return;
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
        foreach (AnimatorSetTrigger a in animators) a.SetTrigger();
        controller.StartGame();
        cam.Priority = 0;
        foreach (var v in startObjects) v.SetActive(true);
    }
}
