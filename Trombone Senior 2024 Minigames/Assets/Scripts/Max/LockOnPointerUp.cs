using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LockOnPointerUp : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Vector2 maxDistanceFromSlider;
    [SerializeField] Vector2 difference;
    [SerializeField] bool withinBounds;
    [SerializeField] float sliderDecayRate;
    public void ResetValue() => slider.value = 0;
    public void LockSlider()
    {
        //ResetValue();
        slider.interactable = false;
    }

    public void UnlockSlider()
    {
        slider.interactable = true;
    }
    //make a vector 2 for distance away your mouse can be from the center of the slider while your mouse is down for it to still work
    //once you are too far, slowly move it back to the start at a fixed rate
    //make user click again to use slider? maybe??? idk?

    private void Update()
    {
        Debug.Log($"{Input.mousePosition} {slider.transform.position}");

        difference = Input.mousePosition - slider.transform.position;
        difference = new Vector2(Mathf.Abs(difference.x), Mathf.Abs(difference.y));

        withinBounds = !(difference.x > maxDistanceFromSlider.x || difference.y > maxDistanceFromSlider.y);

        if (!withinBounds)
        {
            if (slider.interactable) LockSlider();
            if (slider.value != 0)
            {
                int direction = slider.value > 0 ? -1 : 1;
                slider.value += direction * sliderDecayRate * Time.deltaTime;

                if (Mathf.Abs(slider.value - 0) <= 0.05f) slider.value = 0;
            }
        }

        if (withinBounds && !slider.interactable) UnlockSlider();
    }
}
