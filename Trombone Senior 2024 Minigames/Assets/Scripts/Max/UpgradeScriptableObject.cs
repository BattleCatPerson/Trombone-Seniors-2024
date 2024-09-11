using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade Item", order = 1)]
public class UpgradeScriptableObject : ScriptableObject
{
    public int cost;
    public int id;
    public string name;
    public Sprite sprite;
}
