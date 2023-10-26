using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JacksonGameManager : MonoBehaviour
{
    public static int score;
    [Header("Spawning")]
    [SerializeField] float spawnRateInitial;
    [SerializeField] float spawnRate;
    [SerializeField] float spawnRateTimer;
    [SerializeField] float minSpawnRate;

    [SerializeField] float timeToSpeedUp;
    [SerializeField] float spawnRateDecrease;
    [SerializeField] float speedUpTimer;

    [SerializeField] List<Transform> spawnPoints;

    [Header("Objects")]
    [SerializeField] Fox firstObject;
    [SerializeField] Fox secondObject;
    [SerializeField] bool firstOrSecond;

    [Header("Food Sources")]
    [SerializeField] Transform foodSource;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        score = 0;
        spawnRateInitial = spawnRate;
        Spawn();
    }

    void Update()
    {
        scoreText.text = score.ToString();
        spawnRateTimer += Time.deltaTime;
        speedUpTimer += Time.deltaTime;

        if (FoodArea.gameOver) return;

        if (spawnRateTimer > spawnRate)
        {
            Spawn();
            spawnRateTimer = 0f;
        }

        if (speedUpTimer >= timeToSpeedUp)
        {
            SpeedUp();
            speedUpTimer = 0f;
        }
    }

    public void Spawn()
    {
        Fox g = firstOrSecond ? firstObject : secondObject;

        int spawnIndex = Random.Range(0, spawnPoints.Count);
        Transform spawn = spawnPoints[spawnIndex];
        
        var clone = Instantiate(g, spawn.position, spawn.rotation);
        clone.SetTarget(foodSource);

        firstOrSecond = !firstOrSecond;
    }

    public void SpeedUp()
    {
        spawnRate -= spawnRateDecrease;
        spawnRate = Mathf.Clamp(spawnRate, minSpawnRate, spawnRateInitial);
    }
}
