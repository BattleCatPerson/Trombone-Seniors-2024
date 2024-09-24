using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableArcadeButton : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] LoadData data;
    void Start()
    {
        Upgrade(data.unlockedUpgrades);
    }

    void Update()
    {
        
    }
    public void Upgrade(List<CosmeticData.Upgrade> upgrades)
    {
        foreach (var u in upgrades)
        {
            int id = u.id;
            if (id == 7)
            {
                button.SetActive(true);
            }
        }


    }
}
