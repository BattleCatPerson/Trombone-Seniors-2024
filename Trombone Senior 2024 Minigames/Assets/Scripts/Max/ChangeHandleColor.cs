using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChangeHandleColor : MonoBehaviour
{
    [SerializeField] UnityEngine.Color color1;
    [SerializeField] UnityEngine.Color color2;
    [SerializeField] Image handle;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI text;
    void Update()
    {
        handle.color = UnityEngine.Color.Lerp(color1, color2, slider.value / slider.maxValue);
        text.text = slider.value.ToString();
    }
}
