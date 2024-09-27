using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionToMainScene : MonoBehaviour
{
    public static bool transitionIn = false;
    [SerializeField] Animator animator;
    [SerializeField] List<Button> buttonsToDisable;
    [SerializeField] AnimatorSetTrigger setTrigger;
    void Start()
    {
        if (transitionIn)
        {
            animator.SetTrigger("In");
            transitionIn = false;
        }
    }

    public void SetTransitionIn() => transitionIn = true;
    public void DisableButtons()
    {
        foreach (Button b in buttonsToDisable) b.interactable = false;
    }
    public void DisableCanvasGroup() => setTrigger.SetTrigger();
}
