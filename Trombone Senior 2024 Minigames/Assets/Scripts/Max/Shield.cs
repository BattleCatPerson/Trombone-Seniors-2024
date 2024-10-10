using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float rotation;
    [SerializeField] bool canRotate;

    [Header("Laser")]
    [SerializeField] LayerMask mask;
    [SerializeField] int hits;
    [SerializeField] int hitsToShoot;
    int baseHitsToShoot;
    [SerializeField] bool shooting;
    [SerializeField] int count;
    public int Count => count;
    public bool Shooting => shooting;
    [SerializeField] AnimatorSetTrigger setTrigger;
    [SerializeField] Transform laserOrigin;
    [SerializeField] GameObject laserLine;
    [SerializeField] LineRenderer laserRenderer;
    [SerializeField] Transform laserPoint;
    [SerializeField] RadialSlider shieldSlider;
    [SerializeField] MaxCharacterController controller;
    [SerializeField] float points;
    [SerializeField] PoliceCarFollow follow;
    [SerializeField] PoliceCarAudio policeCarAudio;
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera policeCam;
    [SerializeField] CinemachineBrain brain;
    [SerializeField] Transform mainCamera;
    [SerializeField] bool cameraMoving;
    [SerializeField] AnimatorSetTrigger policeCarTrigger;
    [SerializeField] AnimatorSetTrigger policeNumberTrigger;
    [Header("Sound")]
    [SerializeField] AudioSource openSource;
    [SerializeField] AudioClip open;
    [SerializeField] AudioClip close;
    [SerializeField] AudioSource laserSource;
    [SerializeField] AudioClip explodeEffect;
    [Header("Scrap")]
    [SerializeField] CollectibleManager collectibleManager;

    void Start()
    {
        MaxGameManager.instance.restartEvent.AddListener(ResetShieldAnimation);
        MaxGameManager.instance.restartEvent.AddListener(ResetShield);
        MaxGameManager.instance.loadEvent.AddListener(Upgrade);

        baseHitsToShoot = hitsToShoot;
    }

    void Update()
    {
        //if (hitting)
        //{
        //    RaycastHit2D p = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, mask);
        //}
        if (cameraMoving && mainCamera.position == policeCam.transform.position)
        {
            policeCarTrigger.SetTrigger();
            cameraMoving = false;
        }
        if (!canRotate) return;
        transform.eulerAngles = Vector3.forward * rotation;

        if (Input.GetKey(KeyCode.Q)) rotation += 180 * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) rotation -= 180 * Time.deltaTime;
    }

    public void ChargeShield()
    {
        hits++;
        if (hits == hitsToShoot) SwitchToShooting();
    }

    public void SwitchToShooting()
    {
        shooting = true;
        hits = 0;
        setTrigger.SetTrigger();
        MaxGameManager.instance.shieldActive = false;
        //disable laser production of police car
    }

    public void Shoot()
    {
        Physics2D.SyncTransforms();
        RaycastHit2D p = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, mask);
     
        //freeze the shield rotation
        canRotate = false;
        shieldSlider.EnableRotation(false);
        laserSource.Play();
        if (p.collider)
        {
            //destroy the police car, play animation or something liek that
            Debug.Log("LASER HIT POLICE CAR");
            float distance = Vector3.Distance(p.point, laserOrigin.position);
            Debug.Log("Distance " + distance);
            //laserOrigin.position = p.point;
            laserPoint.position = p.point;
            laserRenderer.SetPosition(1, Vector3.right * (laserPoint.localPosition.x));
            laserRenderer.gameObject.SetActive(true);
            //laserPoint.localPosition = Vector3.right * distance;
            policeCam.Priority = 12;
            cameraMoving = true;
            Time.timeScale = 0.5f;
            //Time.timeScale = 0f;
            brain.m_DefaultBlend.m_Time = 0.25f;

            //disable laser shooting
            p.collider.GetComponentInChildren<ProjectileSpawner>().DisableShooting();
            controller.AddPoints(points);
            controller.Freeze(true);
            policeNumberTrigger.SetTrigger();
            follow.stop = true;
            policeCarAudio.StopAllSounds();
            openSource.PlayOneShot(explodeEffect);

            //Scrap Collect
            collectibleManager.CollectScrap();
            count++;
        }
        else
        {
            laserRenderer.SetPosition(1, Vector3.right * 20);
        }

    }
    
    public void ResetShield()
    {
        canRotate = true;
        shieldSlider.EnableRotation(true);
        shooting = false;
    }

    public void EnableParticle() => laserLine.SetActive(true);
    public void DisableLaser()
    {
        laserLine.SetActive(false);
        laserRenderer.gameObject.SetActive(false);
    }
    public void ResetShieldAnimation()
    {
        policeCarTrigger.ResetTrigger();
        hits = 0;
        count = 0;
    }

    public void PlayOpenSound() => openSource.PlayOneShot(open);
    public void PlayCloseSound() => openSource.PlayOneShot(close);
    public void StopLaserSource() => laserSource.Stop();
    public void Upgrade(List<CosmeticData.Upgrade> upgrades)
    {
        foreach (var u in upgrades)
        {
            if (u.id == 5 && u.enabled)
            {
                hitsToShoot = 3;
            }
        }
    }
    public void UpdateHitsToShoot(int thousands)
    {
        hitsToShoot = Mathf.Max((int)(-10 * Mathf.Pow(.95f, thousands) + 15), baseHitsToShoot); 
    }
}
