using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] SpriteRenderer boardSprite;
    [SerializeField] Image gameOverImage;
    [SerializeField] ListOfIdsToSprites pairs;
    [SerializeField] int currentId;
    [SerializeField] string fileName;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] ListOfIdsToTrailColor pairs2;
    [Header("Saving")]
    [SerializeField] MaxCharacterController characterController;
    [SerializeField] CollectibleManager collectibleManager;
    [SerializeField] Shield shield;
    FileHandler fileHandler;
    CosmeticData data;
    void Start()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        data = fileHandler.Load();
        if (data != null) currentId = data.selectedId;
        else currentId = -1;

        foreach (IdSpritePair i in pairs.pairs)
        {
            if (i.id == currentId)
            {
                playerSprite.sprite = i.sprites[0];
                gameOverImage.sprite = i.sprites[0];
                boardSprite.sprite = i.sprites[1];
                break;
            }
        }

        foreach (IdToColorPair i in pairs2.pairs)
        {
            if (i.id == currentId)
            {
                UnityEngine.Color c = i.color;
                trailRenderer.startColor = c;
                trailRenderer.endColor = new UnityEngine.Color(c.r, c.g, c.b, 0);
                break;
            }
        }
    }

    public void UpdateSkinStats()
    {
        int id = data.selectedId;
        List<float> stats = new List<float>() { 1, characterController.Score, collectibleManager.CollectiblesCollected, shield.Count};
        bool found = false;
        int ind = -1;
        for (int i = 0; i < data.skinStats.Count; i++)
        {
            if (data.skinStats[i].id == id)
            {
                found = true;
                ind = i;
                break;
            }
        }
        if (!found)
        {
            CosmeticData.IdToStats c = new();
            c.id = id;
            c.stats = stats;
            data.skinStats.Add(c);
        }
        else
        {
            for (int i = 0; i < stats.Count; i++)
            {
                if (i >= data.skinStats[ind].stats.Count) data.skinStats[ind].stats.Add(stats[i]);
                else data.skinStats[ind].stats[i] += stats[i];
            }
        }

        fileHandler.Save(data);
    }
}
