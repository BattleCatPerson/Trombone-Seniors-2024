using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CosmeticType
{
    costume, trail
}

public class CosmeticShop : MonoBehaviour, IWardrobe
{
    [SerializeField] CosmeticType type;
    [SerializeField] List<int> cosmetics;
    [SerializeField] List<int> unlocked;

    public void Load(CosmeticData data)
    {
        unlocked = data.ReturnList(type);
        unlocked.Add(1);
    }

    public void Save(CosmeticData data)
    {
        
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}
