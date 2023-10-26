using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class DragManager : MonoBehaviour
{
    [SerializeField] GameObject draggedObject;
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
        if (FoodArea.gameOver) return;

        var activeTouches = Touch.activeTouches;
        Vector3 mousePosition;
        mousePosition = activeTouches.Count > 0 ? Camera.main.ScreenToWorldPoint(activeTouches[0].screenPosition) : Vector3.zero;
        foreach (Touch touch in activeTouches)
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(touch.screenPosition));
            List<Collider2D> results = new();
            int count = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.screenPosition), new ContactFilter2D().NoFilter(), results);
            if (count > 0 && touch.phase == TouchPhase.Began && results[0].TryGetComponent<Fox>(out Fox f))
            {
                draggedObject = results[0].gameObject;
                if (f.Grabbable) continue;
                f.Grab(true);
            }
            //else if (count == 0 && touch.phase != TouchPhase.Began) hoveredObject = null;
        }

        if (activeTouches.Count == 0 && draggedObject)
        {
            draggedObject.GetComponent<Fox>().Grab(false);
            draggedObject = null;
        }
        // 10/23/2023 next thing to add make it so that the hovered object follows the touch position! 
        if (draggedObject) draggedObject.transform.position = new Vector2(mousePosition.x, mousePosition.y);
        Debug.Log(activeTouches.Count);
    }

    void OnTouchInput()
    {
        Debug.Log("SDFOJSDFKLMPFKSD");
    }

}

