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
    [SerializeField] Vector2 pos;
    [SerializeField] Vector2 relPos;
    [SerializeField] RectTransform cursor;
    [SerializeField] RectTransform center;


    [Header("Loop Detection")]
    [SerializeField] bool direction;
    [SerializeField] float startAngle;
    [SerializeField] float angleAccumulation;
    [SerializeField] float timeToReset;
    public enum Quadrant
    {
        I, II, III, IV
    }

    [SerializeField] Quadrant quadrant;

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
            cursor.anchoredPosition = activeTouches[0].screenPosition;
            pos = activeTouches[0].screenPosition;
            relPos = cursor.anchoredPosition - center.anchoredPosition;
            norm = relPos.normalized;
            GetQuadrant();
            ChangeAngleOffQuadrant();
        }
    }

    public void GetQuadrant()
    {
        if (relPos.y >= 0)
        {
            if (relPos.x >= 0) quadrant = Quadrant.I;
            else quadrant = Quadrant.II;
        }
        else
        {
            if (relPos.x <= 0) quadrant = Quadrant.III;
            else quadrant = Quadrant.IV;
        }
    }

    public void ChangeAngleOffQuadrant()
    {
        float tan = Mathf.Atan(relPos.y / relPos.x) * 180 / MathF.PI;
        switch (quadrant)
        {
            case Quadrant.I:
                angle = tan;
                break;
            case Quadrant.II:
                angle = 90 + (90 + tan);
                break;
            case Quadrant.III:
                angle = 180 + tan;
                break;
            case Quadrant.IV:
                angle = 270 + (90 + tan);
                break;
        }
    }
}
