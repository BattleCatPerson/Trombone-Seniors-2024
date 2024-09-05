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
    
    public int CollectiblesCollected => collectiblesCollected;

    [SerializeField] int minSpawn;
    [SerializeField] int maxSpawn;
    [SerializeField] float maxDistanceFromPoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] List<TextMeshProUGUI> currentRunText;
    [SerializeField] List<GameObject> collectiblesSpawned;
    [SerializeField] bool gameOver;

    [Header("Sound")]
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource source;
    //if this is on just use raycasts instead of touch!
    [SerializeField] bool windowsBuild;

    [Header("Scrap")]
    [SerializeField] int minScrap;
    [SerializeField] int maxScrap;
    [SerializeField] CollectibleManager collectibleManager;
    [SerializeField] TextMeshProUGUI scrapText;
    [SerializeField] List<TextMeshProUGUI> currentScrapText;
    [SerializeField] int scrap;
    [SerializeField] int scrapCollected;

    private void Start()
    {
        layer = 1 << layer;
        floorLayer = 1 << floorLayer;
        if (!PlayerPrefs.HasKey("Max Collectibles")) PlayerPrefs.SetInt("Max Collectibles", 0);
        else collectibles = PlayerPrefs.GetInt("Max Collectibles");
        
        if (!PlayerPrefs.HasKey("Max Collectibles")) PlayerPrefs.SetInt("Scrap", 0);
        else scrap = PlayerPrefs.GetInt("Scrap");

        lineRenderer.positionCount = 0;

        text.text = $"{collectibles}";
        scrapText.text = $"{scrap}";

        foreach (TextMeshProUGUI t in currentRunText) t.text = $"{collectiblesCollected}";
        foreach (TextMeshProUGUI t in currentScrapText) t.text = $"{scrapCollected}";
        MaxGameManager.instance.restartEvent.AddListener(ResetGame);
    }
    void Update()
    {
        if (gameOver) return;
        if (!windowsBuild)
        {
            var activeTouches = Touch.activeTouches;

            foreach (Touch touch in activeTouches)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.screenPosition), Vector2.zero, Mathf.Infinity, layer);
                if (hit.collider)
                {
                    Collect(hit.collider.gameObject);
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layer);
            if (hit.collider)
            {
                Collect(hit.collider.gameObject);
            }
        }
        
        
    }
    public void GameOver()
    {
        gameOver = true;
        UpdateCollectibles();
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
        scrapCollected = 0;
        foreach (TextMeshProUGUI t in currentRunText) t.text = $"{collectiblesCollected}";
        foreach (TextMeshProUGUI t in currentScrapText) t.text = $"{scrapCollected}";
    }

    public void Collect(GameObject g)
    {
        collectibles++;
        collectiblesCollected++;
        Destroy(g);
        text.text = $"{collectibles}";
        foreach(TextMeshProUGUI t in currentRunText) t.text = $"{collectiblesCollected}";
        source.PlayOneShot(clip);
    }

    public void CollectScrap()
    {
        int sNum = Random.Range(minScrap, maxScrap + 1);
        scrap += sNum;
        scrapCollected += sNum;
        scrapText.text = $"{scrap}";
        foreach (TextMeshProUGUI t in currentScrapText) t.text = $"{scrapCollected}";
    }

    public void ResetGame() => gameOver = false;
}
