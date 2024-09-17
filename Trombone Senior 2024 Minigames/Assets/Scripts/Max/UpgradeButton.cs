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
    public GameObject equipButton;
    public bool canEquip;
    void Start()    
    {
        text.text = upgrade.name;
        image.sprite = upgrade.sprite;

        if (canEquip && sold) equipButton.SetActive(true);
    }
    private void Update()
    {
        if (canEquip && sold && !equipButton.activeInHierarchy) equipButton.SetActive(true);
    }
    public void Purchase()
    {
        sold = true;
        soldPicture.SetActive(true);
        button.interactable = false;
    }
}
