using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class LockOnPointerUp : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] RectTransform top;
    [SerializeField] RectTransform right;
    [SerializeField] Vector2 maxDistanceFromSlider;
    [SerializeField] float multiplier;
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
    private void Start()
    {
        float x = right.position.x - slider.transform.position.x;
        float y = top.position.y - slider.transform.position.y;

        maxDistanceFromSlider = new(x * multiplier, y * multiplier);
    }
    private void Update()
    {
        bool touching = Input.touchCount > 0;
        if (touching == false)
        {
            if (slider.interactable) LockSlider();
            withinBounds = false;
        }
        else
        {
            Dictionary<float, Touch> touches = new();
            List<float> distances = new();

            foreach (Touch t in Touch.activeTouches)
            {
                float d = Vector2.Distance(t.screenPosition, slider.transform.position);
                distances.Add(d);
                touches[d] = t;
            }
            if (distances.Count > 0)
            {
                distances.Sort();
                Debug.Log(distances[0]);
                Debug.Log(touches[distances[0]]);
                Vector3 touchPos = touches[distances[0]].screenPosition;

                difference = touchPos - slider.transform.position;
                difference = new Vector2(Mathf.Abs(difference.x), Mathf.Abs(difference.y));

                withinBounds = !(difference.x > maxDistanceFromSlider.x || difference.y > maxDistanceFromSlider.y);
            }
            else
            {
                if (slider.interactable) LockSlider();
                withinBounds = false;
            }
        }
        

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
