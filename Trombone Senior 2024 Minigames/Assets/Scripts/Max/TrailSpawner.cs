using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSpawner : MonoBehaviour
{
    [SerializeField] GameObject trailPrefab;
    [SerializeField] float velocity;
    [SerializeField] List<Gradient> colors;
    [SerializeField] float interval;
    [SerializeField] Transform spawn;
    [SerializeField] float yVariance;
    [SerializeField] float speed;
    float lastTime = -1f;
    void Start()
    {
        
    }

    void Update()
    {
        int tInt = Mathf.FloorToInt(Time.time);
        if (tInt % interval == 0 && tInt != lastTime)
        {
            lastTime = tInt;
            //sopaw
            int count = Random.Range(1, 6);
            for (int i = 0; i < count; i++) { Spawn(); }
        }
    }

    public void Spawn()
    {
        GameObject t = Instantiate(trailPrefab, spawn.position + Vector3.up * Random.Range(-yVariance, yVariance), Quaternion.identity);
        t.GetComponentInChildren<TrailRenderer>().colorGradient = colors[Random.Range(0, colors.Count)];
        t.GetComponent<Trailer>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;
    }
}
