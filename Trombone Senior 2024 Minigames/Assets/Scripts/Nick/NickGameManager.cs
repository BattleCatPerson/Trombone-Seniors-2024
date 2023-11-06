using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class NickGameManager : MonoBehaviour
{
    [Serializable]
    public class Range
    {
        public int start;
        public int end;
    }

    [SerializeField] Range xRange;
    [SerializeField] Slider slider;
    [SerializeField] Transform slide;

    [SerializeField] List<Rigidbody> walls;
    [SerializeField] float wallMoveSpeed;
    [SerializeField] Transform spawnPoint;

    [SerializeField] float spawnSpeed;
    [SerializeField] float timer;

    [SerializeField] float timeToSpeedUp;
    [SerializeField] float speedUpTimer;
    [SerializeField] float speedDecrease;
    [SerializeField] float minSpeed;

    [SerializeField] bool gameOver;
    [SerializeField] int score;

    [SerializeField] GameObject lastCollided;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] GameObject newHighScoreText;

    void Start()
    {
        SpawnWall();
        gameOverPanel.SetActive(false);
        newHighScoreText.SetActive(false);
        if (!PlayerPrefs.HasKey("Nick High Score")) PlayerPrefs.SetInt("Nick High Score", 0);
    }

    void Update()
    {
        slide.localPosition = new(slider.value * xRange.end, slide.localPosition.y);

        if (gameOver) return;
        timer += Time.deltaTime;
        speedUpTimer += Time.deltaTime;
        if (timer >= spawnSpeed)
        {
            SpawnWall();
            timer = 0f;
        }
        if (speedUpTimer >= timeToSpeedUp)
        {
            spawnSpeed -= speedDecrease;
            if (spawnSpeed < minSpeed) spawnSpeed = minSpeed;
            speedUpTimer = 0f;
        }

        scoreText.text = $"{score}";
        finalScoreText.text = $"{score}";
    }

    public void SpawnWall()
    {
        Rigidbody rb = walls[Random.Range(0, walls.Count)];
        Rigidbody clone = Instantiate(rb, spawnPoint.position, spawnPoint.rotation);

        clone.velocity = -Vector3.forward * wallMoveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.transform.root.gameObject != lastCollided)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            score++;

            lastCollided = other.transform.root.gameObject;
            Destroy(other.transform.root.gameObject, 2f);
        }
    }

    public void StopGame()
    {
        Debug.Log("You Lose");
        gameOver = true;
        gameOverPanel.SetActive(true);

        if (score > PlayerPrefs.GetInt("Nick High Score"))
        {
            newHighScoreText.SetActive(true);
            PlayerPrefs.SetInt("Nick High Score", score);
        }
    }
}
