using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompendiumButton : MonoBehaviour
{
    public int id;
    [SerializeField] Image image;
    [SerializeField] Button button;
    [SerializeField] GameObject questionMark;
    private void Awake()
    {
        DisableButton();
    }

    public void UpdatePanel()
    {
        SkinInfoPanel.instance.EnablePanel(id);
    }

    public void DisableButton()
    {
        image.color = UnityEngine.Color.black;
        button.interactable = false;
        questionMark.SetActive(true);
    }

    public void EnableButton()
    {
        image.color = UnityEngine.Color.white;
        button.interactable = true;
        questionMark.SetActive(false);
    }
}
