using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Random = UnityEngine.Random;
using Mathf = UnityEngine.Mathf;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class MaxCharacterController : MonoBehaviour
{
    [Serializable]
    public class CharToSpriteDictionary
    {
        public char number;
        public Sprite sprite;
    }

    [Header("Physics")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 direction;
    [SerializeField] Vector2 rampDirection;
    [SerializeField] float speed;
    [SerializeField] float downwardForceRate;
    [SerializeField] bool colliding;
    [SerializeField] float initialForce;
    public bool Colliding => colliding;
    [SerializeField] bool canFlip;
    [SerializeField] bool touchingRamp;
    [SerializeField] int collidersTouching;
    [SerializeField] List<GameObject> colliders;
    [SerializeField] float rampImpulse;
    [SerializeField] float rampImpulseIncrease;
    [SerializeField, Tooltip("On Ground Hit: reset background and get rid of sun")] UnityEvent hitFloorEvent;
    [Header("Rotation")]
    [SerializeField] Slider slider;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float drag;
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] float verticalDistanceToFloor;
    [SerializeField] LayerMask layer;
    [SerializeField] int layerInt;
    [SerializeField] float baseCameraSize;
    [SerializeField] float maxCameraSize;
    [Header("Score")]
    [SerializeField] int flips;
    [SerializeField] float accumulatedAngle;
    [SerializeField] float score;
    [SerializeField] float scorePerFlip;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] GameObject newHighScoreText;
    [Header("Game Over")]
    [SerializeField] float angleDeviation;
    [SerializeField] bool gameOver;
    [Header("Ragdoll")]
    [SerializeField] Rigidbody2D maxRb;
    [SerializeField] float launchForce;
    [Header("Score Text")]
    [SerializeField] SpriteRenderer oneSprite;
    [SerializeField] SpriteRenderer tenSprite;
    [SerializeField] List<CharToSpriteDictionary> dictionary;
    [SerializeField] List<GameObject> comboTexts;
    [SerializeField] int bonusActive;
    [SerializeField] List<int> flipIntervals;
    [SerializeField] List<float> statusBonuses;
    [SerializeField] List<string> names;
    [SerializeField] TextMeshProUGUI baseScoreText;
    [SerializeField] TextMeshProUGUI bonusScoreText;
    [SerializeField] CanvasGroup scoreTextGroup;
    [SerializeField] float fadeRate;
    [SerializeField] List<UnityEvent> comboEvents;
    [SerializeField] CollectibleManager collectibleManager;
    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip confirmClip;
    [SerializeField] float pitch;
    [SerializeField] float pitchIncrease;
    [SerializeField] float pitchFloor;
    [SerializeField] AudioSource rampSource;
    [SerializeField] AudioClip rampClip;
    [SerializeField] AudioClip deathClip;
    void Start()
    {
        maxRb.isKinematic = true;
        SetSprites(false);
        Combos(false);
        bonusActive = -1;
        scoreTextGroup.alpha = 0;
        rb.velocity = Vector2.right * initialForce;
        gameOverPanel.SetActive(false);
        newHighScoreText.SetActive(false);
        if (!PlayerPrefs.HasKey("Max High Score")) PlayerPrefs.SetInt("Max High Score", 0);
        canFlip = false;
        pitch = audioSource.pitch;
        pitchFloor = pitch;
    }

    void FixedUpdate()
    {
        if (gameOver) return;
        scoreText.text = $"Score: {score}";
        finalScoreText.text = $"Score: {score}";

        collidersTouching = colliders.Count;
        colliding = collidersTouching > 0;
        if (collidersTouching == 0)
        {
            colliding = false;
            direction = Vector2.zero;
        }
        if (scoreTextGroup.alpha > 0)
        {
            scoreTextGroup.alpha -= fadeRate * Time.fixedDeltaTime;
        }
        var activeTouches = Touch.activeTouches;
        if (direction.magnitude > 0)
        {
            //rb.velocity = (Vector2)direction * speed;
            //rb.AddForce(direction * speed * TimefixedDeltaTime);
            transform.right = direction;
        }

        if (canFlip)
        {
            if (slider.value > 0) rotationSpeed += acceleration * slider.value * Time.fixedDeltaTime;
            else if (slider.value < 0) rotationSpeed += deceleration * slider.value * Time.fixedDeltaTime;
            else rotationSpeed -= drag * Time.fixedDeltaTime;
            rotationSpeed = Mathf.Clamp(rotationSpeed, 0, maxRotationSpeed);
            transform.eulerAngles += Vector3.forward * rotationSpeed * Time.fixedDeltaTime;
            accumulatedAngle += rotationSpeed * Time.fixedDeltaTime;

            if (accumulatedAngle >= 360)
            {
                flips++;
                Debug.Log("PLAY SOUND");
                audioSource.PlayOneShot(clip);
                audioSource.pitch += pitchIncrease;
                accumulatedAngle = 0;
                SetSprites(true);
            }
        }

        Vector2 point = (Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layer).point);
        verticalDistanceToFloor = Vector2.Distance((Vector2) transform.position + Vector2.down * transform.localScale, point);

        if (verticalDistanceToFloor > 1f)
        {
            vCam.m_Lens.OrthographicSize = baseCameraSize + verticalDistanceToFloor;
        }
        else vCam.m_Lens.OrthographicSize = baseCameraSize;
        vCam.m_Lens.OrthographicSize = Mathf.Clamp(vCam.m_Lens.OrthographicSize, baseCameraSize, maxCameraSize);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, Mathf.Infinity, layer);
        if (hit.collider && hit.collider.gameObject.CompareTag("Ramp"))
        {
            if (!touchingRamp && !canFlip)
            {
                touchingRamp = true;
                rb.velocity = direction * rampImpulse;
                rampSource.PlayOneShot(rampClip);
                Debug.Log($"Impulse {rampImpulse}");
            }
        }
        else if ((!hit.collider || (layer == (layer | (1 << hit.collider.gameObject.layer)))) && touchingRamp)
        {
            canFlip = true;
            touchingRamp = false;

            collectibleManager.SpawnCollectibles(rb.velocity, transform.position, -Physics2D.gravity.y * rb.gravityScale);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerInt && canFlip)
        {
            canFlip = false;
            hitFloorEvent?.Invoke();
        }
        if (!colliders.Contains(collision.gameObject))
        {
            colliders.Add(collision.gameObject);
        }
        if (!colliding)
        {
            float floorRotation = collision.transform.eulerAngles.z;
            float initialRotation = transform.eulerAngles.z;
            float newRotation = Mathf.Abs(floorRotation - initialRotation) > Mathf.Abs(floorRotation - (initialRotation + 360)) ? initialRotation + 360 : initialRotation;
            float deviation = Mathf.Abs(floorRotation - newRotation);
            if (deviation > angleDeviation && !collision.gameObject.CompareTag("Ramp"))
            {
                StopGame();
                return;
            }

            rotationSpeed = 0;
            if (flips > 0)
            {
                scoreTextGroup.alpha = 1;
                score += scorePerFlip * flips;
                baseScoreText.text = $"+{scorePerFlip * flips}";
                if (bonusActive != -1)
                {
                    score += statusBonuses[bonusActive];
                    bonusScoreText.text = $"+{statusBonuses[bonusActive]} {names[bonusActive]} bonus";
                }
                else
                {
                    bonusScoreText.text = "";
                }
                rampImpulse += rampImpulseIncrease;
                audioSource.PlayOneShot(confirmClip);
            }
            flips = 0;
            accumulatedAngle = 0;
            bonusActive = -1;
        }
        Vector2 v = Vector2.Perpendicular(-collision.GetContact(0).normal);
        rb.velocity = v * rb.velocity.magnitude;
        direction = v;
        SetSprites(false);

        audioSource.pitch = pitchFloor;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!colliders.Contains(collision.gameObject))
        {
            colliders.Add(collision.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        colliders.Remove(collision.gameObject);

        //if (collision.gameObject.CompareTag("Ramp"))
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
        //    if (hit.collider && !hit.collider.gameObject.CompareTag("Ramp"))
        //    {
        //        canFlip = true;
        //    }
        //}
    }

    public void SetSprites(bool b)
    {
        if (b)
        {
            string str = flips.ToString();
            char ones;
            char tens;
            if (flips >= 10)
            {
                ones = str[1];
                tens = str[0];
            }
            else
            {
                ones = str[0];
                tens = '0';
            }

            foreach (CharToSpriteDictionary c in dictionary)
            {
                if (c.number == ones) oneSprite.sprite = c.sprite;
                else if (c.number == tens) tenSprite.sprite = c.sprite;
            }
            Combos(true);
            return;
        }
        oneSprite.sprite = null;
        tenSprite.sprite = null;
        Combos(false);
    }

    public void Combos(bool b)
    {
        if (b)
        {
            Combos(false);
            for (int i = flipIntervals.Count - 1; i > -1; i--)
            {
                if (flips >= flipIntervals[i])
                {
                    comboTexts[i].SetActive(true);
                    comboEvents[i]?.Invoke();
                    bonusActive = i;
                    return;
                }
            }
        }
        else
        {
            foreach (GameObject g in comboTexts) g.SetActive(false);
        }

    }

    public void StopGame()
    {
        gameOver = true;
        colliding = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        maxRb.isKinematic = false;
        maxRb.GetComponent<Collider2D>().enabled = true;
        maxRb.AddTorque(-launchForce);
        SetSprites(false);
        rampSource.PlayOneShot(deathClip);
        gameOverPanel.SetActive(true);
        if (score > PlayerPrefs.GetInt("Max High Score"))
        {
            newHighScoreText.SetActive(true);
            PlayerPrefs.SetInt("Max High Score", (int) score);
        }

        collectibleManager.UpdateCollectibles();
    }
}
