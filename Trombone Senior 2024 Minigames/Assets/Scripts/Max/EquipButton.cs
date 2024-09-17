using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipButton : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] bool upgradeEnabled;
    [SerializeField] UpgradeShop upgradeShop;
    [SerializeField] GameObject enabledImage;
    private void Start()
    {
        upgradeEnabled = upgradeShop.UpgradeEquipped(id);
        enabledImage.SetActive(upgradeEnabled);
    }
    public void Toggle()
    {
        upgradeEnabled = !upgradeEnabled;
        upgradeShop.EnableItem(id, upgradeEnabled);
        enabledImage.SetActive(upgradeEnabled);
    }
}
