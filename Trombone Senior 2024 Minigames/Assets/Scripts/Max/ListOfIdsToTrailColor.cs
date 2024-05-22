using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class IdToColorPair
{
    public int id;
    public UnityEngine.Color color;
}
public class ListOfIdsToTrailColor : MonoBehaviour
{
    public List<IdToColorPair> pairs;
}
