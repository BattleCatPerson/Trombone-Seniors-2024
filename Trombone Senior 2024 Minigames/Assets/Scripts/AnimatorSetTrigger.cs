using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetTrigger : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string trigger;
    [SerializeField] string resetTrigger;
    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }
    public void SetTrigger() => animator.SetTrigger(trigger);
    public void ResetTrigger() => animator.SetTrigger(resetTrigger);
}
