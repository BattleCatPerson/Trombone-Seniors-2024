using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    [SerializeField] int scrap;
    [SerializeField] List<int> unlockedUpgrades;
    [SerializeField] int upgradeLevel;
    [SerializeField] List<UpgradeButton> buttons;
    [SerializeField] string fileName;
    [SerializeField] Cosmetic initialCosmetic;
    [SerializeField] FileHandler fileHandler;
    [SerializeField] CosmeticData data;
    //money booster
    //speed booster
    //arcade game pass
    //luck booster
    //ramp boosters for more air time
    //higher energy gain from lasers
    //increase angle for landing
    //bonus booster (double points from nice cool bonus)
    void Start()
    {
        scrap = PlayerPrefs.GetInt("Scrap");
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        data = fileHandler.Load();
        if (data == null) data = new CosmeticData(initialCosmetic);

        unlockedUpgrades = data.upgrades;
    }

    void Update()
    {
        
    }

    public void Upgrade(UpgradeButton button)
    {
        var obj = button.Upgrade;
        if (scrap >= obj.cost && !unlockedUpgrades.Contains(obj.id))
        {
            unlockedUpgrades.Add(obj.id);
            fileHandler.Save(data);
            scrap -= obj.cost;
            //Disable
        }
    }

    private void OnApplicationQuit()
    {
    }
}
