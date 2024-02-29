using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;
public enum CosmeticType
{
    costume, trail
}
public enum Rarity
{
    common, rare, superRare
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
    [Header("Results")]
    [SerializeField] CanvasGroup resultsPanel;
    [SerializeField] Image resultImage;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI rarityText;
    [SerializeField] bool resultPanelActive;
    [SerializeField] float growthRate;
    [SerializeField] GameObject continueButton;
    [Header("Cost")]
    [SerializeField] int cost;
    [SerializeField] int currency;

    List<Rarity> chances;

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
    public void DisableUi() => rollPanel.SetActive(false);
    public void EnableUi() => rollPanel.SetActive(true);
    public void Roll()
    {
        Rarity selected = chances[Random.Range(0, chances.Count)];
        List<Cosmetic> valid = new();

        foreach (Cosmetic c in cosmetics)
        {
            if (c.rarity == selected) valid.Add(c);
        }
        Cosmetic final = valid[Random.Range(0, valid.Count)];
        resultText.text = final.name;
        rarityText.text = final.rarity.ToString();

        List<int> ids = new();
        foreach (Cosmetic c in unlocked) ids.Add(c.id);
        if (!ids.Contains(final.id))
        {
            unlocked.Add(final);
            wardrobe.AddToPanel(final.id);
        }
    }

    public void Start()
    {
        chances = new();
        for (int i = 0; i < COMMON_PERCENT; i++) chances.Add(Rarity.common);
        for (int i = 0; i < RARE_PERCENT; i++) chances.Add(Rarity.rare);
        for (int i = 0; i < SUPER_RARE_PERCENT; i++) chances.Add(Rarity.superRare);
        currency = PlayerPrefs.GetInt("Max Collectibles");
    }

    public void Update()
    {
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
    }

    public void SetPanelActive() => resultPanelActive = true;
    public void SetPanelStandby()
    {
        resultsPanel.alpha = 0f;
        resultText.text = "";
        rarityText.text = "";
        resultImage.sprite = null;
        resultsPanel.blocksRaycasts = false;
        continueButton.SetActive(false);
    }
}
