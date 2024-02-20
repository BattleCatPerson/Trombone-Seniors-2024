using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetTrigger : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string trigger;

    public void SetTrigger() => animator.SetTrigger(trigger);
}
