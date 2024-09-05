using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoliceCarFollow : MonoBehaviour
{
    [Serializable]
    public class AnimatorToSprite
    {
        public RuntimeAnimatorController animator;
        public RuntimeAnimatorController droneAnimator;
        public Sprite sprite;
        public Material laserMaterial;
        public UnityEngine.Color pointColor;
        public Gradient flameGradient;
        public Sprite deployerSprite;
        public UnityEngine.Color deployerColor;
        public UnityEngine.Color countdownColor;
        public bool outline = true;
    }

    [SerializeField] Transform target;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;
    [SerializeField] Vector3 offset;
    [SerializeField] MaxCharacterController controller;
    public bool stop;
    private bool started = false;
    [Header("Replace Sprite")]
    [SerializeField] List<AnimatorToSprite> carAnimators;
    [SerializeField] AnimatorToSprite defaultCar;
    [SerializeField] AnimatorToSprite currentCar;
    public AnimatorToSprite CurrentCar => currentCar;
    [SerializeField] Animator animator;
    [SerializeField] CinemachineVirtualCamera policeCam;
    [SerializeField] Transform mainCam;
    [SerializeField] Transform mainVCam;
    [SerializeField] bool camMoving;
    [SerializeField] CinemachineBrain brain;
    [SerializeField] ProjectileSpawner spawner;
    [SerializeField] SwitchBackground switchBackground;
    [SerializeField] List<ParticleSystem> flames;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Material outlineMaterial;
    [SerializeField] Material defaultMaterial;
    public bool outline;
    Sprite currentSprite;
    [Header("Audio")]
    [SerializeField] PoliceCarAudio pAudio;
    private void Start()
    {
        MaxGameManager.instance.restartEvent.AddListener(Disable);
        MaxGameManager.instance.restartEvent.AddListener(ResetSprite);
        currentCar = defaultCar;
    }
    public void Enable() => started = true;
    public void Disable() => started = false;
    public void ResetSprite()
    {
        //reset to default
        animator.runtimeAnimatorController = defaultCar.animator;
        spawner.Assign(defaultCar);
        foreach (ParticleSystem p in flames)
        {
            var col = p.colorOverLifetime;
            col.enabled = true;
            col.color = defaultCar.flameGradient;
        }
        switchBackground.Switch(defaultCar.sprite);
    }
    public void ResetOnGameOver()
    {
        policeCam.Priority = 0;
        Time.timeScale = 1f;
        pAudio.StopAllSounds();
    }

    void FixedUpdate()
    {
        if (!started) return;
        if (camMoving && mainCam.position == mainVCam.position)
        {
            camMoving = false;
            Time.timeScale = 1f;
            brain.m_DefaultBlend.m_Time = 2f;
            controller.Freeze(false);
            stop = false;
        }
        if (!stop) transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, smoothTime);

    }
    public void Replace()
    {
        Console.WriteLine("CHANG ECHANGE CHANGE!");
        RuntimeAnimatorController anim = animator.runtimeAnimatorController;
        AnimatorToSprite animToSprite = carAnimators[Random.Range(0, carAnimators.Count)];
        while (animToSprite.animator == anim)
        {
            animToSprite = carAnimators[Random.Range(0, carAnimators.Count)];
        }
        currentCar = animToSprite;
        RuntimeAnimatorController random = animToSprite.animator;
        Material m = animToSprite.laserMaterial;

        animator.runtimeAnimatorController = random;
        currentSprite = animToSprite.sprite;
        spawner.Assign(animToSprite);
        foreach (ParticleSystem p in flames)
        {
            var col = p.colorOverLifetime;
            col.enabled = true;
            col.color = animToSprite.flameGradient;
        }

        if (animToSprite.outline && outline) spriteRenderer.material = outlineMaterial;
        else spriteRenderer.material = defaultMaterial;
        //switchBackground.Switch(animToSprite.sprite);
    }
    public void Switch() => switchBackground.Switch(currentSprite);
    public void ResetCamAndTime()
    {
        policeCam.Priority = 0;
        camMoving = true;
    }

    public void EnableShooting() => spawner.EnableShooting();
}
