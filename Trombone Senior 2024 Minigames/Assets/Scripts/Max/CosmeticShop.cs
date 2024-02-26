using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public enum CosmeticType
{
    costume, trail
}
public enum Rarity
{
    common, rare, superRare
}

public class CosmeticShop : MonoBehaviour, IWardrobe
{
    [Serializable]
    public class Cosmetic
    {
        public int id;
        public Sprite sprite;
        public Rarity rarity;
    }

    [SerializeField] CosmeticType type;
    [SerializeField] List<Cosmetic> cosmetics;
    [SerializeField] List<int> unlocked;
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
    
    public void Roll()
    {
        Rarity selected = chances[Random.Range(0, chances.Count)];
        List<Cosmetic> valid = new();

        foreach (Cosmetic c in cosmetics)
        {
            if (c.rarity == selected) valid.Add(c);
        }
        Cosmetic final = valid[Random.Range(0, valid.Count)];

        if (!unlocked.Contains(final.id)) unlocked.Add(final.id);
    }

    public void Start()
    {
        chances = new();
        for (int i = 0; i < COMMON_PERCENT; i++) chances.Add(Rarity.common);
        for (int i = 0; i < RARE_PERCENT; i++) chances.Add(Rarity.rare);
        for (int i = 0; i < SUPER_RARE_PERCENT; i++) chances.Add(Rarity.superRare);
    }
}
