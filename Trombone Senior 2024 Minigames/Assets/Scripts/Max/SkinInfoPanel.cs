using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinInfoPanel : MonoBehaviour
{
    public static SkinInfoPanel instance;
    [SerializeField] Wardrobe wardrobe;
    [SerializeField] ListOfIdsToSprites sprites;
    [SerializeField] Image skinImage;
    [SerializeField] TextMeshProUGUI skinName;
    [SerializeField] TextMeshProUGUI skinTotalRuns;
    [SerializeField] TextMeshProUGUI skinTotalPoints;
    [SerializeField] TextMeshProUGUI skinTotalMoney;
    [SerializeField] TextMeshProUGUI skinPoliceCarsExploded;
    [SerializeField] CanvasGroup canvasGroup;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        DisablePanel();
    }
    public void UpdatePanel(int id)
    {
        skinImage.sprite = sprites.pairs[id + 1].sprites[0];
        List<float> stats = new();
        bool found = false;
        foreach (var pair in wardrobe.data.skinStats)
        {
            if (pair.id == id)
            {
                stats = pair.stats;
                found = true;
            }
        }
        if (!found)
        {
            stats = new() { 0, 0, 0, 0 };
        }

        for (int i = 1; i < 5; i++)
        {
            if (i > stats.Count) stats.Add(0);
        }
        wardrobe.SaveData();
        skinName.text = wardrobe.MatchIdToName(id);
        skinTotalRuns.text = $"Total Runs: {stats[0]}";
        skinTotalPoints.text = $"Total Points: {stats[1]}";
        skinTotalMoney.text = $"Total: {stats[2]}";
        skinPoliceCarsExploded.text = $"Total Police Cars Blown Up: {stats[3]}";
    }

    public void EnablePanel(int id)
    {
        UpdatePanel(id);
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void DisablePanel()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
