using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBackground : MonoBehaviour
{
    [SerializeField] SpriteRenderer currentOverlay;
    [SerializeField] SpriteRenderer replacementOverlay;
    [SerializeField] AnimatorSetTrigger animator;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void Switch(Sprite s)
    {
        replacementOverlay.sprite = s;
        animator.SetTrigger();
    }
    public void Set()
    {
        currentOverlay.sprite = replacementOverlay.sprite;
    }
}
