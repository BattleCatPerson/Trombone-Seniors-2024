using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticData
{
    [Serializable]
    public class IdToStats
    {
        public int id;
        public List<float> stats;
    }
    [Serializable]
    public class Upgrade
    {
        public int id;
        public bool enabled;
    }

    public List<Cosmetic> costumes;
    public List<Cosmetic> trails;
    public int selectedId = -1;
    public int sortId = 0;
    public List<IdToStats> skinStats;
    public List<Upgrade> upgrades;
    public List<Cosmetic> ReturnList(CosmeticType type)
    {
        if (type == CosmeticType.costume) return costumes;
        if (type == CosmeticType.trail) return trails;
        return null;
    }

    public CosmeticData()
    {
        costumes = new();
        trails = new();
        selectedId = -1;
        sortId = 0;
        skinStats = new();
        upgrades = new();
    }

    public CosmeticData(Cosmetic initialCosmetic)
    {
        costumes = new();
        costumes.Add(initialCosmetic);
        trails = new();
        selectedId = -1;
        sortId = 0;
        skinStats = new();
        upgrades = new();
    }
}
