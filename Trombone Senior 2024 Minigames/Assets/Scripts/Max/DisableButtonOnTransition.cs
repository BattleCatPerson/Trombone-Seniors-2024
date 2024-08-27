using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButtonOnTransition : MonoBehaviour
{
    [SerializeField] bool tutorial;
    [SerializeField] Button button;
    [SerializeField] Animator animator;
    [SerializeField] TutorialStepCounter stepCounter;
    [SerializeField] bool forward;
    void Update()
    {
        if (tutorial) button.interactable = !animator.IsInTransition(0) && !((forward && stepCounter.step == 5) || (!forward && stepCounter.step == 1));
        else button.interactable = !animator.IsInTransition(0);
    }
}
