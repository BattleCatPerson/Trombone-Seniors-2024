using System;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Random = UnityEngine.Random;


public class DetectLoop : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] Vector2 norm;
    void Start()
    {
        
    }

    void Update()
    {
        //when you tap for the first time in the flip sequence, get the vector from the center of the screen to the point
        //as you move finger around detect the direction from the origin to your finger?
        var activeTouches = Touch.activeTouches;
        if (activeTouches.Count > 0)
        {
            //change from screen position to rect position (origin at center not at bottom)
            norm = activeTouches[0].screenPosition.normalized;
            angle = Mathf.Atan(norm.y / norm.x);
        }

    }
}
