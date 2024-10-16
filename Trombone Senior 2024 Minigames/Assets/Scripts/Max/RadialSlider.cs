using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
//using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Random = UnityEngine.Random;
using Mathf = UnityEngine.Mathf;
using UnityEngine.UI;
public class RadialSlider : MonoBehaviour
{
    [SerializeField] float value;
    [SerializeField] float radius;
    [SerializeField] Vector2 position;
    [SerializeField] float referenceAngle;
    [SerializeField] RectTransform handle;
    [SerializeField] RectTransform parent;
    [SerializeField] bool withinBounds;
    [SerializeField] float enterDistance;
    [SerializeField] float holdExitDistance;
    [SerializeField] float enterMultiplier;
    [SerializeField] RectTransform top;
    [SerializeField] Shield shield;
    [SerializeField] bool canRotate;
    [SerializeField] MaxGameManager gameManager;
    [SerializeField] bool cTExists;
    [SerializeField] Vector3 currentPos;
    void Start()
    {
        float y = top.position.y - handle.transform.position.y;

        enterDistance = y * enterMultiplier;
        holdExitDistance = enterDistance * 1.5f;
    }

    void Update()
    {
        // if pressing down and within enterDistance, set onHandle to true
        // once onHandle is true, you can be within the movedistance to move the handle
        // if you release or you move past moveDistance, set onHandle to false
        if (!canRotate || gameManager.pauseActive) return;
        Touch currentTouch = new();

        if (Input.touchCount > 0)
        {
            Dictionary<float, Touch> touches = new();
            List<float> di = new();

            foreach (Touch t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    float d = Vector2.Distance(t.position, parent.transform.position);
                    touches[d] = t;
                    di.Add(d);
                }
            }
            di.Sort();
            if (di.Count > 0)
            {
                currentTouch = touches[di[0]];
                Vector3 touchPos = currentTouch.position;
                Vector2 difference = touchPos - parent.transform.position;
                difference = new Vector2(Mathf.Abs(difference.x), Mathf.Abs(difference.y));
                withinBounds = !(difference.x > enterDistance || difference.y > enterDistance);

                cTExists = withinBounds;
            }

            if (cTExists)
            {
                touches = new();
                List<float> distances = new();

                foreach (Touch t in Input.touches)
                {
                    float d = Vector2.Distance(t.position, parent.transform.position);
                    distances.Add(d);
                    touches[d] = t;
                }
                distances.Sort();
                currentTouch = touches[distances[0]];
                Vector3 touchPos = currentTouch.position;
                Vector2 difference = touchPos - parent.transform.position;
                difference = new Vector2(Mathf.Abs(difference.x), Mathf.Abs(difference.y));
                withinBounds = !(difference.x > holdExitDistance || difference.y > holdExitDistance);

                if (withinBounds)
                {
                    Vector2 touchDifference = touchPos - parent.transform.position;
                    float touchAngle = Mathf.Atan(touchDifference.y / touchDifference.x) * 180 / Mathf.PI;

                    bool right = touchPos.x - parent.position.x >= 0;
                    bool up = touchPos.y - parent.position.y >= 0;
                    if (right && !up) touchAngle = 360f + touchAngle;
                    else if ((!right && up) || (!right && !up)) touchAngle = 180f + touchAngle;

                    value = touchAngle;
                }
                else
                {
                    cTExists = false;
                }
            }
        }

        currentPos = currentTouch.position;

        position = ReturnPosition(value);
        handle.anchoredPosition = position;
        shield.rotation = value;

        Vector2 referencePositionPositive = ReturnPosition(value + referenceAngle);
        Vector2 referencePositionNegative = ReturnPosition(value - referenceAngle);
    }

    public Vector2 ReturnPosition(float v)
    {
        return new Vector2(radius * Mathf.Cos(v * Mathf.PI / 180), radius * Mathf.Sin(v * Mathf.PI / 180));
    }

    public void EnableRotation(bool b)
    {
        canRotate = b;
    }
}
