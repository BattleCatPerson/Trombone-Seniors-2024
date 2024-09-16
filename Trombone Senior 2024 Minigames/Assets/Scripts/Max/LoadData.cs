using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    [SerializeField] FileHandler fileHandler;
    [SerializeField] CosmeticData data;
    public CosmeticData Data => data;
    [SerializeField] Cosmetic initialCosmetic;
    [SerializeField] string fileName;
    [SerializeField] List<int> unlockedUpgrades;
    void Awake()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        data = fileHandler.Load();
        if (data == null) data = new CosmeticData(initialCosmetic);

        unlockedUpgrades = data.upgrades;
    }

    void Update()
    {
        
    }
}
