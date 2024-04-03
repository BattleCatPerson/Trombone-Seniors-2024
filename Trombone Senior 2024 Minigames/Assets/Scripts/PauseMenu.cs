using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject baseMenu;
    [SerializeField] GameObject soundMenu;

    public void SwitchToSound()
    {
        baseMenu.SetActive(false);
        soundMenu.SetActive(true);
    }

    public void SwitchToBase()
    {
        baseMenu.SetActive(true);
        soundMenu.SetActive(false);
    }
}
