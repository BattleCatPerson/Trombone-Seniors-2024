using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacksonGameManager : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] float spawnRate;
    [SerializeField] float minSpawnRate;
    [SerializeField] float timeToSpeedUp;
    [SerializeField] float spawnRateDecrease;

    [Header("Objects")]
    [SerializeField] GameObject firstObject;
    [SerializeField] GameObject secondObject;

    void Start()
    {
        
    }

    void Update()
    {
        //just do a timer
    }
}
