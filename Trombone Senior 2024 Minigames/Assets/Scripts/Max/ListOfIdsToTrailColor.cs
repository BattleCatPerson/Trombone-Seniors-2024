using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ListOfIdsToTrailColor : MonoBehaviour
{
    [Serializable]
    public class IdToColorPair
    {
        public int id;
        public UnityEngine.Color color;
    }

    [SerializeField] List<IdToColorPair> pairs;
}
