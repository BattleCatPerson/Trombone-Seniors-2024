using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetTrigger : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string trigger;
    [SerializeField] string resetTrigger;

    public void SetTrigger() => animator.SetTrigger(trigger);
    public void ResetTrigger() => animator.SetTrigger(resetTrigger);
}
