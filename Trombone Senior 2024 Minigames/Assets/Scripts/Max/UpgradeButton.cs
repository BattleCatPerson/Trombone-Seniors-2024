using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UpgradeScriptableObject upgrade;
    public UpgradeScriptableObject Upgrade => upgrade;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image image;
    [SerializeField] UpgradeShop shop;
    [SerializeField] bool sold;
    [SerializeField] GameObject soldPicture;
    [SerializeField] Button button;
    void Start()    
    {
        text.text = upgrade.name;
        image.sprite = upgrade.sprite;
    }
    public void Purchase()
    {
        sold = true;
        soldPicture.SetActive(true);
        button.interactable = false;
    }
}
