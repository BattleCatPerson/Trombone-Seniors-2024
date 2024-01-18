using System;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] int layer;
    private void Start()
    {
        layer = 1 << layer;
    }
    void Update()
    {
        var activeTouches = Touch.activeTouches;

        foreach (Touch touch in activeTouches)
        {
            if (touch.phase != TouchPhase.Began) continue;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.screenPosition), Vector2.zero, Mathf.Infinity, layer);
            Debug.Log(hit.collider);
        }
    }
}
