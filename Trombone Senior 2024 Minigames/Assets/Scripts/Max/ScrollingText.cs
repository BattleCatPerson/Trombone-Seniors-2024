using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    [SerializeField] float separation;
    [SerializeField] List<RectTransform> texts;
    [SerializeField] float scrollRate;
    [SerializeField] float maxPosition;
    void Start()
    {
        
    }

    void Update()
    {
        foreach (RectTransform r in texts) r.localPosition += Vector3.right * scrollRate * Time.deltaTime;
        if (texts[0].localPosition.x > maxPosition)
        {
            RectTransform temp = texts[0];
            temp.localPosition = texts[texts.Count - 1].localPosition - Vector3.right * separation;
            texts.RemoveAt(0);
            texts.Add(temp);
        }
    }
}
