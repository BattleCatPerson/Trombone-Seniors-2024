using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoliceCarFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;
    [SerializeField] Vector3 offset;
    private bool started = false;
    [Header("Replace Sprite")]
    [SerializeField] List<RuntimeAnimatorController> carAnimators;
    [SerializeField] Animator animator;
    [SerializeField] CinemachineVirtualCamera policeCam;
    [SerializeField] Transform mainCam;
    [SerializeField] Transform mainVCam;
    [SerializeField] bool camMoving;
    [SerializeField] CinemachineBrain brain;
    [SerializeField] ProjectileSpawner spawner;
    private void Start()
    {
        MaxGameManager.instance.restartEvent.AddListener(Disable);
    }
    public void Enable() => started = true;
    public void Disable() => started = false;

    void FixedUpdate()
    {
        if (!started) return;
        if (camMoving && mainCam.position == mainVCam.position)
        {
            camMoving = false;
            Time.timeScale = 1f;
            brain.m_DefaultBlend.m_Time = 2f;
        }
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, smoothTime);

    }
    public void Replace()
    {
        Console.WriteLine("CHANG ECHANGE CHANGE!");
        RuntimeAnimatorController anim = animator.runtimeAnimatorController;
        RuntimeAnimatorController random = carAnimators[Random.Range(0, carAnimators.Count)];

        while (random == animator)
        {
            random = carAnimators[Random.Range(0, carAnimators.Count)];
        }
        animator.runtimeAnimatorController = random;
    }

    public void ResetCamAndTime()
    {
        policeCam.Priority = 0;
        camMoving = true;
    }

    public void EnableShooting() => spawner.EnableShooting();
}
