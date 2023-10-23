using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class DragManager : MonoBehaviour
{
    [SerializeField] GameObject hoveredObject;
    protected void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    protected void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    protected void Update()
    {
        var activeTouches = Touch.activeTouches;
        foreach (Touch touch in activeTouches)
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(touch.screenPosition));
            List<Collider2D> results = new();
            int count = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.screenPosition), new ContactFilter2D().NoFilter(), results);
            if (count > 0 && touch.phase == TouchPhase.Began) hoveredObject = results[0].gameObject;
            else if (count == 0 && touch.phase != TouchPhase.Began) hoveredObject = null;

        }

        if (activeTouches.Count == 0) hoveredObject = null;

        // 10/23/2023 next thing to add make it so that the hovered object follows the touch position! 
    }

    void OnTouchInput()
    {
        Debug.Log("SDFOJSDFKLMPFKSD");
    }

}

