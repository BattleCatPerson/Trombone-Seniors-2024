using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinInfoPanel : MonoBehaviour
{
    [SerializeField] Wardrobe wardrobe;
    [SerializeField] ListOfIdsToSprites sprites;
    [SerializeField] Image skinImage;
    [SerializeField] TextMeshProUGUI skinName;
    [SerializeField] TextMeshProUGUI skinTotalRuns;
    [SerializeField] TextMeshProUGUI skinTotalPoints;
    [SerializeField] TextMeshProUGUI skinTotalMoney;

    private void Start()
    {
        UpdatePanel(-1);
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
            stats = new() { 0, 0, 0 };
        }

        skinName.text = wardrobe.MatchIdToName(id);
        skinTotalRuns.text = $"Total Runs: {stats[0]}";
        skinTotalPoints.text = $"Total Points: {stats[1]}";
        skinTotalMoney.text = $"Total: {stats[2]}";
    }
}
