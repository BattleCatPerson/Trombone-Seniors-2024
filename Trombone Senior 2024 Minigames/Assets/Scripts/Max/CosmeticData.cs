using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticData
{
    public List<int> costumes;
    public List<int> trails;

    public List<int> ReturnList(CosmeticType type)
    {
        if (type == CosmeticType.costume) return costumes;
        if (type == CosmeticType.trail) return trails;
        return null;
    }

    public CosmeticData()
    {
        costumes = new();
        trails = new();
    }
}
