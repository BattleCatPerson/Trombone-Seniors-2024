using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBackground : MonoBehaviour
{
    [SerializeField] SpriteRenderer currentBackground;
    [SerializeField] SpriteRenderer replacementBackground;
    [SerializeField] AnimatorSetTrigger animator;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void Switch(Sprite s)
    {
        replacementBackground.sprite = s;
        animator.SetTrigger();
    }
    public void Set()
    {
        currentBackground.sprite = replacementBackground.sprite;
    }
}
