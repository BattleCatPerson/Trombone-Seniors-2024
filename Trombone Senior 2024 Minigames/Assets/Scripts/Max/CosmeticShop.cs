using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;
public enum CosmeticType
{
    costume, trail
}
public enum Rarity
{
    Common, Rare, SuperRare
}
[Serializable]
public class Cosmetic
{
    public int id;
    public Rarity rarity;
    public string name;
}
public class CosmeticShop : MonoBehaviour, IWardrobe
{
    [Header("Cosmetic Pool")]
    [SerializeField] CosmeticType type;
    [SerializeField] List<Cosmetic> cosmetics;
    [SerializeField] List<Cosmetic> unlocked;
    [SerializeField] Wardrobe wardrobe;
    [SerializeField] GameObject rollPanel;
    [SerializeField] bool luckUpgrade;
    [SerializeField] TextMeshProUGUI commonChanceText;
    [SerializeField] TextMeshProUGUI sRareChanceText;
    [SerializeField] GameObject upgradedText;
    int commonCount = 1;
    int rareCount = 0;
    int superRareCount = 0;
    [Header("Results")]
    [SerializeField] CanvasGroup resultsPanel;
    [SerializeField] Image resultPlayerImage;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI rarityText;
    [SerializeField] bool resultPanelActive;
    [SerializeField] float growthRate;
    [SerializeField] GameObject continueButton;
    [Header("Cost")]
    [SerializeField] int cost;
    [SerializeField] int currency;
    [SerializeField] int scrap;
    [SerializeField] Button rollButton;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] TextMeshProUGUI scrapText;
    [SerializeField] TextMeshProUGUI costText;
    [Header("Box Launching")]
    [SerializeField] float launchForce;
    [SerializeField] float torque;
    [SerializeField] Rigidbody lid;
    [SerializeField] Vector3 originalPosition;
    [SerializeField] bool waitingForLaunchInput;
    [SerializeField] Animator animator;
    [SerializeField] bool windowsBuild;
    List<Rarity> chances;
    [Header("Scrap Compensation")]
    [SerializeField] int commonScrap;
    [SerializeField] int rareScrap;
    [SerializeField] int superRareScrap;
    [SerializeField] Sprite scrapSprite;

    const int COMMON_PERCENT = 70;
    const int RARE_PERCENT = 25;
    const int SUPER_RARE_PERCENT = 5;
    
    public void Load(CosmeticData data)
    {
        unlocked = data.ReturnList(type);
    }

    public void Save(CosmeticData data)
    {
        
    }
    public void EnableLuckUpgrade()
    {
        luckUpgrade = true;
        commonChanceText.text = "65";
        sRareChanceText.text = "10";
        upgradedText.SetActive(true);
    }
    public void DisableUi() => rollPanel.SetActive(false);
    public void EnableUi() => rollPanel.SetActive(true);
    public void Roll()
    {
        Rarity selected = chances[Random.Range(0, chances.Count)];
        List<Cosmetic> valid = new();
        List<int> ids = new();
        foreach (Cosmetic c in unlocked) ids.Add(c.id);

        foreach (Cosmetic c in cosmetics)
        {
            if (c.rarity == selected && !ids.Contains(c.id)) valid.Add(c);
        }

        if (valid.Count == 0)
        {
            int scrapBonus = selected == Rarity.Common ? commonScrap : (selected == Rarity.Rare ? rareScrap : superRareScrap);
            scrap += scrapBonus;
            PlayerPrefs.SetInt("Scrap", scrap);
            resultText.text = scrapBonus.ToString() + " Scrap";
            rarityText.text = "You have all " + ReturnRarityString(selected) + " skins";
            resultPlayerImage.sprite = scrapSprite;
        }
        else
        {
            Cosmetic final = valid[Random.Range(0, valid.Count)];
            resultText.text = final.name;
            rarityText.text = ReturnRarityString(final.rarity);
            resultPlayerImage.sprite = wardrobe.MatchIdToSprite(final.id)[0];

            if (!ids.Contains(final.id))
            {
                unlocked.Add(final);
                wardrobe.AddToPanel(final.id);
                wardrobe.UpdatePanel();
                wardrobe.SetCompendiumTexts(commonCount, rareCount, superRareCount);
                wardrobe.EnableCompendiumButton(final.id);
            }
        }
    }
    private void Awake()
    {
        foreach (var v in cosmetics)
        {
            if (v.rarity == Rarity.Common) commonCount++;
            else if (v.rarity == Rarity.Rare) rareCount++;
            else superRareCount++;
        }
    }
    public void Start()
    {
        chances = new();
        int finalCommon = luckUpgrade ? COMMON_PERCENT - 5 : COMMON_PERCENT;
        int finalSuperRare = luckUpgrade ? SUPER_RARE_PERCENT + 5 : SUPER_RARE_PERCENT;
        for (int i = 0; i < finalCommon; i++) chances.Add(Rarity.Common);
        for (int i = 0; i < RARE_PERCENT; i++) chances.Add(Rarity.Rare);
        for (int i = 0; i < finalSuperRare; i++) chances.Add(Rarity.SuperRare);
        currency = PlayerPrefs.GetInt("Max Collectibles");

        lid.isKinematic = true;
        originalPosition = lid.transform.localPosition;
        currencyText.text = currency.ToString();
        costText.text = $"x{cost}";

        if (!PlayerPrefs.HasKey("Max Collectibles")) PlayerPrefs.SetInt("Scrap", 0);
        else scrap = PlayerPrefs.GetInt("Scrap");
        scrapText.text = scrap.ToString();
    }

    public void Update()
    {
        if (waitingForLaunchInput && ((Touch.activeTouches.Count > 0 && !windowsBuild) || (Input.GetMouseButtonDown(0) && windowsBuild)))
        {
            //disable waiting
            waitingForLaunchInput = false;
            //launch lid
            lid.isKinematic = false;
            lid.AddForce((new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)).normalized * launchForce), ForceMode.Impulse);
            lid.AddTorque((new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f)).normalized * torque));
            //do the stuff it did before
            SetPanelActive();
        }
        if (resultPanelActive)
        {
            resultsPanel.blocksRaycasts = true;

            if (resultsPanel.alpha < 1f)
            {
                resultsPanel.alpha += growthRate * Time.deltaTime;
            }
            else
            {
                resultPanelActive = false;
                continueButton.SetActive(true);
            }
        }
        rollButton.interactable = currency >= cost;
        currencyText.text = currency.ToString();
    }

    public void SetPanelActive()
    {
        scrapText.text = scrap.ToString();
        resultPanelActive = true;
    }
    public void SetPanelStandby()
    {
        resultsPanel.alpha = 0f;
        resultText.text = "";
        rarityText.text = "";
        resultPlayerImage.sprite = null;
        resultsPanel.blocksRaycasts = false;
        continueButton.SetActive(false);
    }

    public void Charge()
    {
        currency -= cost;
        PlayerPrefs.SetInt("Max Collectibles", currency);
    }

    public void ReturnLid()
    {
        lid.isKinematic = true;
        lid.transform.eulerAngles = Vector3.zero;
        lid.transform.localPosition = originalPosition;
        animator.SetTrigger("Roll");
    }

    public void SetLidReady() => waitingForLaunchInput = true;
    public string ReturnRarityString(Rarity r)
    {
        if (r == Rarity.SuperRare) return "Super Rare";
        else return r.ToString();
    }

    public List<int> ReturnRarityNumbers()
    {
        return new() { commonCount, rareCount, superRareCount};
    }
}
