using System;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Cinemachine;
public class MaxGameManager : MonoBehaviour
{
    [SerializeField] MaxCharacterController controller;
    [SerializeField] GameObject startPanel;
    [SerializeField] List<GameObject> startObjects;
    [SerializeField] List<AnimatorSetTrigger> animators;
    public static bool started;
    public static bool gameOver;
    [SerializeField] CinemachineVirtualCamera cam;
    void Start()
    {
        started = false;
        gameOver = false;
        foreach (var v in startObjects) v.SetActive(false);
    }

    void Update()
    {
        if (started) return;
        var activeTouches = Touch.activeTouches;

        if (activeTouches.Count > 0)
        {
            started = true;
            foreach (AnimatorSetTrigger a in animators) a.SetTrigger();
            controller.StartGame();
            cam.Priority = 0;
            startPanel.SetActive(false);
            foreach (var v in startObjects) v.SetActive(true);
        }
    }
}
