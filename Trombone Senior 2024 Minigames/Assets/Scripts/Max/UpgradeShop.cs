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
    [SerializeField] List<string> descriptions;
    [SerializeField] string fileName;
    [SerializeField] Cosmetic initialCosmetic;
    [SerializeField] FileHandler fileHandler;
    [SerializeField] CosmeticData data;
    [SerializeField] UpgradeDialogue dialogue;
    [SerializeField] string purchaseDialogue;
    [Header("Purchase Panel")]
    [SerializeField] AnimatorSetTrigger panelOpenTrigger;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Button confirmButton;
    [SerializeField] UpgradeScriptableObject selectedItem;
    [SerializeField] UpgradeButton upgradeButton;
    [SerializeField] AnimatorSetTrigger notEnoughMoneyTrigger;
    [SerializeField] AnimatorSetTrigger purchaseTrigger;
    [SerializeField] TextMeshProUGUI yourScrap;
    [SerializeField] Animator openShopScreen;
    [SerializeField] CanvasGroup baseCanvasGroup;
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
        unlockedUpgrades.Clear();
        foreach (var i in unlockedUpgrades)
        {
            foreach (UpgradeButton b in buttons) if (b.Upgrade.id == i) b.Purchase();
        }
        yourScrap.text = scrap.ToString();
    }

    void Update()
    {
        yourScrap.text = scrap.ToString();
    }

    public void Upgrade()
    {
        if (scrap >= selectedItem.cost && !unlockedUpgrades.Contains(selectedItem.id))
        {
            unlockedUpgrades.Add(selectedItem.id);
            fileHandler.Save(data);
            scrap -= selectedItem.cost;
            PlayerPrefs.SetInt("Scrap", scrap);
            upgradeButton.Purchase();
            purchaseTrigger.SetTrigger();
            dialogue.DisplayMessage(purchaseDialogue);
            //Disable
        }
        else
        {
            notEnoughMoneyTrigger.SetTriggerDelay();
        }
    }

    public void OpenPanel(UpgradeButton button)
    {
        upgradeButton = button;
        UpgradeScriptableObject obj = button.Upgrade;
        panelOpenTrigger.SetTrigger();
        nameText.text = obj.name;
        costText.text = obj.cost.ToString();
        image.sprite = obj.sprite;
        selectedItem = obj;
        descriptionText.text = descriptions[obj.id].Replace("(nL)", "\n");
    }

    private void OnApplicationQuit()
    {
    }

    public void OpenShop(bool enabled) => openShopScreen.SetBool("Open", enabled);
    public void EnableBaseCanvas(bool enabled) => baseCanvasGroup.interactable = enabled;
}
