using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToMainScene : MonoBehaviour
{
    public static bool transitionIn = false;
    [SerializeField] Animator animator;
    void Start()
    {
        if (transitionIn)
        {
            animator.SetTrigger("In");
            transitionIn = false;
        }
    }

    public void SetTransitionIn() => transitionIn = true;
}
