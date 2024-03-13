using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChangeColors : MonoBehaviour
{
    [Serializable]
    public class ImageColorPair
    {
        public Image i;
        public UnityEngine.Color currentColor;
        public UnityEngine.Color newColor;
    }

    [SerializeField] List<UnityEngine.Color> palette;
    [SerializeField] List<ImageColorPair> pairs;
    [SerializeField] float timeToSwitch;
    [SerializeField] float timer;
    [SerializeField] float transitionTime;
    [SerializeField] bool switching;
    void Start()
    {
        UpdateColors(true);
    }

    void Update()
    {
        if (switching)
        {
            if (timer < transitionTime)
            {
                timer += Time.deltaTime;
                foreach (ImageColorPair p in pairs)
                {
                    p.i.color = UnityEngine.Color.Lerp(p.currentColor, p.newColor, timer / transitionTime);
                }
            }
            else
            {
                switching = false;
                timer = 0;
                foreach (ImageColorPair p in pairs)
                {
                    p.currentColor = p.newColor;
                    p.newColor = UnityEngine.Color.white;
                }
            }
        }
        else if (timer < timeToSwitch)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            switching = true;
            UpdateColors(false);
        }
    }

    public void UpdateColors(bool start)
    {
        if (start)
        {
            foreach (ImageColorPair p in pairs)
            {
                p.currentColor = palette[Random.Range(0, palette.Count)];
                p.i.color = p.currentColor;
            }
        }
        else
        {
            foreach (ImageColorPair p in pairs)
            {
                p.newColor = palette[Random.Range(0, palette.Count)];
            }
        }
        
    }
}
