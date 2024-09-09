using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    [SerializeField] int scrap;
    [SerializeField] List<int> upgradeCosts;
    [SerializeField] int upgradeLevel;
    void Start()
    {
        scrap = PlayerPrefs.GetInt("Scrap");
    }

    void Update()
    {
        
    }

    public void Upgrade()
    {
        if (scrap > upgradeCosts[upgradeLevel])
        {
            scrap -= upgradeCosts[upgradeLevel];
            upgradeLevel++;
            PlayerPrefs.SetInt("Scrap", scrap);
        }
    }
}
