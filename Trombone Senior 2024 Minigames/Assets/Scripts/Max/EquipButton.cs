using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipButton : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] bool upgradeEnabled;
    [SerializeField] UpgradeShop upgradeShop;

    public void Toggle()
    {
        upgradeShop.EnableItem(id, upgradeEnabled);
        upgradeEnabled = !upgradeEnabled;
    }
}
