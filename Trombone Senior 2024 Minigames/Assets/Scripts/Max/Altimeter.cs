using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Altimeter : MonoBehaviour
{
    [SerializeField] float height;
    public float floatHeight;
    [SerializeField] float maxMeterHeight;
    [SerializeField] int floorLayer;
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Vector2 minAndMaxAngles;
    [SerializeField] float currentAngle;
    [SerializeField] RectTransform pointer;
    [SerializeField] float areaWidth;
    [SerializeField] int multiplier;

    float min;
    float max;
    private void Start()
    {
        floorLayer = 1 << floorLayer;
        //currentAngle = minAndMaxAngles[0];
        //multiplier = minAndMaxAngles[1] > minAndMaxAngles[0] ? 1 : -1;

        //if (minAndMaxAngles[0] > minAndMaxAngles[1])
        //{
        //    min = minAndMaxAngles[1];
        //    max = minAndMaxAngles[0];
        //}
        //else
        //{
        //    max = minAndMaxAngles[1];
        //    min = minAndMaxAngles[0];
        //}
    }
    void FixedUpdate()
    {
        //just raycast only to the floor at all times to get the altitude
        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, Mathf.Infinity, floorLayer);
        Vector2 point = hit.point;
        floatHeight = player.position.y - point.y;
        int temp = (int)Mathf.Floor(floatHeight);
        if (hit.collider) height = temp;
        else
        {
            Debug.Log("TEMP:" + temp + "POINT:" + point + "COLLIDER:" + hit.collider);
            //player.GetComponent<Rigidbody2D>().isKinematic = true;
            //player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        text.text = $"Altitude\n{height}";

        //set rotation of the pointer based off height / maxMeterHeight. If height > maxMeterHeight, just make the pointer stay at the max angle.
        // multiply the fraction by the different between the min and max angles and then add that value to the min value in order to get the right angle.

        //float delta = (height / maxMeterHeight) * Mathf.Abs(minAndMaxAngles[1] - minAndMaxAngles[0]);
        //pointer.transform.eulerAngles = Vector3.forward * Mathf.Clamp((minAndMaxAngles[0] + multiplier * delta), min, max);

        pointer.anchoredPosition = Vector3.right * Mathf.Lerp(-areaWidth, areaWidth, floatHeight / maxMeterHeight);
    }
}
