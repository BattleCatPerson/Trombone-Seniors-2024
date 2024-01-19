using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] int layer;
    [SerializeField] GameObject collectiblePrefab;
    [SerializeField] int collectibles;
    private void Start()
    {
        layer = 1 << layer;
        if (!PlayerPrefs.HasKey("Max Collectibles")) PlayerPrefs.SetInt("Max Collectibles", 0);
        else collectibles = PlayerPrefs.GetInt("Max Collectibles");
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
        //get number of collectibles to spawn
        //only spawn when you reach a certain distance
        //spawn them in a random area around player????? idk man im so tired!!!!!!!

        //find the max height the player will reach then spawnt them there!
        // vf = 0, 0 = vo - gt, solve for t, get horizontal velocity and multiply it by t, spawn coins at player position on launch plus the x and at the y value it peaks at.
        // how to get yf??? 
        //area under the graph is integral, so get the area under velocity. we have t, so multiply initial velocity by the calculated t to get the y???

        float prevVelY = -Mathf.Infinity;
        float velY = v0.y;
        float t = 0;
        float step = Time.deltaTime;
        while (true)
        {
            prevVelY = velY;
            velY -= g * step;
            t += step;
            if (velY < prevVelY) break;
        }

        Debug.Log(t);
        
        
    }
}
