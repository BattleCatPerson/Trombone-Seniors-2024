using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSetTrigger : MonoBehaviour
{
    [SerializeField] float minTime;
    [SerializeField] float maxTime;
    [SerializeField] float timer;
    [SerializeField] Animator animator;
    [SerializeField] string trigger;
    void Start()
    {
        timer = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            animator.SetTrigger(trigger);
            timer = Random.Range(minTime, maxTime);
        }
    }
}
