using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButtonOnTransition : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Animator animator;
    [SerializeField] TutorialStepCounter stepCounter;
    [SerializeField] bool forward;
    void Update()
    {
        button.interactable = !animator.IsInTransition(0) && !((forward && stepCounter.step == 4) || (!forward && stepCounter.step == 1));
    }
}
