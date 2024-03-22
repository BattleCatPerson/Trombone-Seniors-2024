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
    [Header("Results")]
    [SerializeField] CanvasGroup resultsPanel;
    [SerializeField] Image resultPlayerImage;
    [SerializeField] Image resultBoardImage;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI rarityText;
    [SerializeField] bool resultPanelActive;
    [SerializeField] float growthRate;
    [SerializeField] GameObject continueButton;
    [Header("Cost")]
    [SerializeField] int cost;
    [SerializeField] int currency;
    [SerializeField] Button rollButton;
    [Header("Box Launching")]
    [SerializeField] float launchForce;
    [SerializeField] float torque;
    [SerializeField] Rigidbody lid;
    [SerializeField] Vector3 originalPosition;
    [SerializeField] bool waitingForLaunchInput;
    [SerializeField] Animator animator;
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
        rarityText.text = ReturnRarityString(final.rarity);
        resultPlayerImage.sprite = wardrobe.MatchIdToSprite(final.id)[0];
        resultBoardImage.sprite = wardrobe.MatchIdToSprite(final.id)[1];
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
        for (int i = 0; i < COMMON_PERCENT; i++) chances.Add(Rarity.Common);
        for (int i = 0; i < RARE_PERCENT; i++) chances.Add(Rarity.Rare);
        for (int i = 0; i < SUPER_RARE_PERCENT; i++) chances.Add(Rarity.SuperRare);
        currency = PlayerPrefs.GetInt("Max Collectibles");

        lid.isKinematic = true;
        originalPosition = lid.transform.localPosition;
    }

    public void Update()
    {
        if (waitingForLaunchInput && Touch.activeTouches.Count > 0)
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
    }

    public void SetPanelActive() => resultPanelActive = true;
    public void SetPanelStandby()
    {
        resultsPanel.alpha = 0f;
        resultText.text = "";
        rarityText.text = "";
        resultPlayerImage.sprite = null;
        resultBoardImage.sprite = null;
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
}
