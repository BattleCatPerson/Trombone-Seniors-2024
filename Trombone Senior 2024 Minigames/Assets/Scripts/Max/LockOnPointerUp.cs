using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LockOnPointerUp : MonoBehaviour
{
    [SerializeField] Slider slider;
    public void ResetValue() => slider.value = 0;
    public void LockSlider()
    {
        ResetValue();
        slider.interactable = false;
    }

    public void UnlockSlider()
    {
        slider.interactable = true;
    }
}
