using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UpgradeScriptableObject upgrade;
    public UpgradeScriptableObject Upgrade => upgrade;
    [SerializeField] TextMeshProUGUI text;
    void Start()    
    {
        text.text = upgrade.name;
    }
}
