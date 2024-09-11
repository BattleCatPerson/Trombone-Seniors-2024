using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    [Header("Purchase Panel")]
    [SerializeField] AnimatorSetTrigger panelOpenTrigger;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Button confirmButton;
    [SerializeField] UpgradeScriptableObject selectedItem;
    //money booster
    //speed booster
    //arcade game pass
    //luck booster
    //ramp boosters for more air time
    //higher energy gain from lasers
    //increase angle for landing
    //bonus booster (double points from nice cool bonus)
    void Awake()
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

    public void Upgrade()
    {
        if (scrap >= selectedItem.cost && !unlockedUpgrades.Contains(selectedItem.id))
        {
            unlockedUpgrades.Add(selectedItem.id);
            fileHandler.Save(data);
            scrap -= selectedItem.cost;
            //Disable
        }
    }

    public void OpenPanel(UpgradeScriptableObject obj)
    {
        panelOpenTrigger.SetTrigger();
        nameText.text = obj.name;
        costText.text = obj.cost.ToString();
        image.sprite = obj.sprite;
        selectedItem = obj;
    }

    private void OnApplicationQuit()
    {
    }
}
