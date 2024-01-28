using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShieldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float movement;
    [SerializeField] bool pressed;
    [SerializeField] Shield shield;

    private void Update()
    {
        if (pressed) shield.rotation += movement * Time.deltaTime; 
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
}
