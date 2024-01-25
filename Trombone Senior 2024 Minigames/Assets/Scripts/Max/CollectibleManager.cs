using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Random = UnityEngine.Random;
public class CollectibleManager : MonoBehaviour
{
    [SerializeField] int layer;
    [SerializeField] int floorLayer;
    [SerializeField] GameObject collectiblePrefab;
    [SerializeField] int collectibles;

    [SerializeField] int minSpawn;
    [SerializeField] int maxSpawn;
    [SerializeField] float maxDistanceFromPoint;
    [SerializeField] LineRenderer lineRenderer;
    private void Start()
    {
        layer = 1 << layer;
        floorLayer = 1 << floorLayer;
        if (!PlayerPrefs.HasKey("Max Collectibles")) PlayerPrefs.SetInt("Max Collectibles", 0);
        else collectibles = PlayerPrefs.GetInt("Max Collectibles");

        lineRenderer.positionCount = 0;
    }
    void Update()
    {
        var activeTouches = Touch.activeTouches;

        foreach (Touch touch in activeTouches)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.screenPosition), Vector2.zero, Mathf.Infinity, layer);
            if (hit.collider)
            {
                collectibles++;
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void UpdateCollectibles() => PlayerPrefs.SetInt("Max Collectibles", collectibles);
    public void SpawnCollectibles(Vector2 v0, Vector2 pos, float g)
    {
        lineRenderer.positionCount = 0;

        float prevVelY = -Mathf.Infinity;
        float velY = v0.y;
        float t = 0;
        float step = Time.fixedDeltaTime;
        Vector2 currentPos = pos;
        List<Vector2> points = new();
        
        while (true)
        {
            prevVelY = velY;
            if (velY < 0)
            {
                Vector2 point = (Physics2D.Raycast(currentPos, Vector2.down, Mathf.Infinity, floorLayer).point);
                float height = currentPos.y - point.y;
                if (height > maxDistanceFromPoint) Spawn(currentPos);
                break;
            }
            currentPos += new Vector2(v0.x * step, velY * step);
            points.Add(currentPos);
            velY -= g * step;
            t += step;
        }
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    public void Spawn(Vector2 pos)
    {
        int count = Random.Range(minSpawn, maxSpawn);
        Instantiate(collectiblePrefab, pos  , transform.rotation);

        //Dictionary<float, float> dict = new();

        //for (int i = 0; i < count; i++)
        //{
        //    float distance = 0;
        //    float r = 0;
        //    while (true)
        //    {
        //        distance = Random.Range(0, maxDistanceFromPoint);
        //        r = (10 * Random.Range(0, 36)) * Mathf.PI / 180f;

        //        if (!CheckInDictionary(dict, distance, r)) break;
        //    }

        //    Vector2 dir = (new Vector2(Mathf.Cos(r), Mathf.Sin(r))).normalized;
        //    Instantiate(collectiblePrefab, pos + dir * distance, transform.rotation);
        //    dict.Add(distance, r);
        //}
    }

    public bool CheckInDictionary(Dictionary<float, float> d, float distance, float rotation)
    {
        if (d.ContainsKey(distance) && d.ContainsValue(rotation)) return true;
        return false;
    }
}
