using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem.EnhancedTouch;
public class CollectibleManager : MonoBehaviour
{
    [SerializeField] int layer;
    [SerializeField] int floorLayer;
    [SerializeField] GameObject collectiblePrefab;
    [SerializeField] int collectibles;
    [SerializeField] int collectiblesCollected;

    [SerializeField] int minSpawn;
    [SerializeField] int maxSpawn;
    [SerializeField] float maxDistanceFromPoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] List<GameObject> collectiblesSpawned;

    //if this is on just use raycasts instead of touch!
    [SerializeField] bool windowsBuild;
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
        if (!windowsBuild)
        {
            var activeTouches = Touch.activeTouches;

            foreach (Touch touch in activeTouches)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.screenPosition), Vector2.zero, Mathf.Infinity, layer);
                if (hit.collider)
                {
                    collectibles++;
                    collectiblesCollected++;
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layer);
            if (hit.collider)
            {
                collectibles++;
                collectiblesCollected++;
                Destroy(hit.collider.gameObject);
            }
        }
        
        text.text = $"{collectibles}";
        gameOverText.text = $"{collectiblesCollected}";
    }

    public void UpdateCollectibles() => PlayerPrefs.SetInt("Max Collectibles", collectibles);
    public void SpawnCollectibles(Vector2 v0, Vector2 pos, float g)
    {
        lineRenderer.positionCount = 0;

        float prevVelY = -Mathf.Infinity;
        float velY = v0.y;
        float step = Time.fixedDeltaTime;
        List<Vector2> points = new();
        
        while (true)
        {
            prevVelY = velY;
            if (velY < 0)
            {
                Vector2 point = (Physics2D.Raycast(pos, Vector2.down, Mathf.Infinity, floorLayer).point);
                float height = pos.y - point.y;
                if (height > maxDistanceFromPoint) Spawn(pos);
                break;
            }
            velY -= g * step;
            pos += new Vector2(v0.x * step, velY * step);
            points.Add(pos);
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
        Dictionary<float, float> dict = new();

        for (int i = 0; i < count; i++)
        {
            float distance = 0;
            float r = 0;
            while (true)
            {
                distance = Random.Range(0, maxDistanceFromPoint);
                r = (10 * Random.Range(0, 36)) * Mathf.PI / 180f;

                if (!CheckInDictionary(dict, distance, r)) break;
            }

            Vector2 dir = (new Vector2(Mathf.Cos(r), Mathf.Sin(r))).normalized;
            collectiblesSpawned.Add(Instantiate(collectiblePrefab, pos + dir * distance, transform.rotation));
            dict.Add(distance, r);
        }
    }

    public bool CheckInDictionary(Dictionary<float, float> d, float distance, float rotation)
    {
        if (d.ContainsKey(distance) && d.ContainsValue(rotation)) return true;
        return false;
    }

    public void DestroyAllActive()
    {
        foreach(var c in collectiblesSpawned) Destroy(c);
        collectiblesSpawned.Clear();
        collectiblesCollected = 0;
    }
}
