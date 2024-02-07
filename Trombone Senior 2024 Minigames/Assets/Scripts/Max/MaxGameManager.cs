using System;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Cinemachine;
public class MaxGameManager : MonoBehaviour
{
    [SerializeField] MaxCharacterController controller;
    [SerializeField] GameObject gamePanel;
    [SerializeField] Animator bank;
    public static bool started;
    [SerializeField] CinemachineVirtualCamera cam;
    void Start()
    {
        started = false;
        gamePanel.SetActive(false);
    }

    void Update()
    {
        if (started) return;
        var activeTouches = Touch.activeTouches;

        if (activeTouches.Count == 0) Debug.Log("NO TOUCHES");
        else
        {
            started = true;
            bank.SetTrigger("Start");
            controller.StartGame();
            cam.Priority = 0;
            gamePanel.SetActive(true);
        }
    }
}
