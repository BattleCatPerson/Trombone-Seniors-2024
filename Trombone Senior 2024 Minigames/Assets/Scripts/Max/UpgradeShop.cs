using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class UpgradeShop : MonoBehaviour
{
    [SerializeField] int scrap;
    [SerializeField] List<CosmeticData.Upgrade> unlockedUpgrades;
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
    [SerializeField] Animator openEquipScreen;
    [SerializeField] CanvasGroup baseCanvasGroup;
    [Header("Equip Panel")]
    [SerializeField] GameObject noEquipText;
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
        bool equipable = false;
        foreach (var i in unlockedUpgrades)
        {
            foreach (UpgradeButton b in buttons)
            {
                if (b.Upgrade.id == i.id)
                {
                    b.Purchase();
                    if (b.canEquip) equipable = true;
                }
            }
        }
        if (equipable) noEquipText.SetActive(false);
        yourScrap.text = scrap.ToString();
    }

    void Update()
    {
        yourScrap.text = scrap.ToString();
    }

    public void Upgrade()
    {
        if (scrap >= selectedItem.cost)
        {
            //&& !unlockedUpgrades.Contains(selectedItem.id)
            foreach (var u in unlockedUpgrades)
            {
                if (u.id == selectedItem.id) return;
            }
            unlockedUpgrades.Add(new CosmeticData.Upgrade(selectedItem.id, true));
            fileHandler.Save(data);
            scrap -= selectedItem.cost;
            PlayerPrefs.SetInt("Scrap", scrap);
            upgradeButton.Purchase();
            purchaseTrigger.SetTrigger();
            dialogue.DisplayMessage(purchaseDialogue);

            foreach (UpgradeButton b in buttons)
            {
                if (b.Upgrade.id == selectedItem.id)
                {
                    b.Purchase();
                    if (b.canEquip && noEquipText.activeInHierarchy) noEquipText.SetActive(false);
                }
            }
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
    public void OpenEquip(bool enabled) => openEquipScreen.SetBool("Open", enabled);
    public void EnableBaseCanvas(bool enabled) => baseCanvasGroup.interactable = enabled;
    public void EnableItem(int id, bool status)
    {
        foreach (var u in data.upgrades) if (u.id == id) { u.enabled = status; break; }
        fileHandler.Save(data);
    }
    public bool UpgradeEquipped(int id)
    {
        foreach (var u in data.upgrades) if (u.id == id && u.enabled) return true;
        return false;
    }
}
