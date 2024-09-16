using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpgradeDialogue : MonoBehaviour
{
    [SerializeField] float timeBetweenCharacters;
    [SerializeField] float timer;
    [SerializeField] string message;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] bool active;
    [SerializeField] int messageCount;
    [SerializeField] int index;
    [SerializeField] Animator animator;
    void Start()
    {
        DisplayMessage("welcome to my humble shop");
    }

    void FixedUpdate()
    {
        if (active)
        {
            if (timer > 0) timer -= Time.fixedDeltaTime;
            else if (timer <= 0)
            {
                text.text += message[index];
                index++;
                timer = timeBetweenCharacters;

                if (index == message.Length)
                {
                    animator.SetBool("Talking", false);
                    active = false;
                }
            }
        }
    }

    public void DisplayMessage(string s = "blurb")
    {
        text.text = "";
        active = true;
        timer = timeBetweenCharacters;
        index = 0;
        message = s;
        animator.SetBool("Talking", true);
    }
}
