using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeButton : MonoBehaviour
{
    [Serializable]
    public class ArcadeGameInfo
    {
        public string title;
        public string description;
        public string highScoreKey;
        public string scene;
        public Sprite image;
    }
    public ArcadeGameInfo info;
    
}
