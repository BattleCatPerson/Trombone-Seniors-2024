using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandingBubble : MonoBehaviour
{
    public float alphaDecreaseRate;
    public Image image;
    public float growthRate;

    [SerializeField] float scale;
    void Start()
    {
        scale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        image.rectTransform.localScale = Vector2.one * scale;
        scale += growthRate * Time.deltaTime;
        image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a - alphaDecreaseRate * Time.deltaTime);
        if (image.color.a <= 0f) Destroy(gameObject);
    }
}
