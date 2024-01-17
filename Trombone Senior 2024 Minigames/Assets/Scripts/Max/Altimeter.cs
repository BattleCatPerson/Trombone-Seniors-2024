using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Altimeter : MonoBehaviour
{
    [SerializeField] float height;
    [SerializeField] float maxMeterHeight;
    [SerializeField] int floorLayer;
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Vector2 minAndMaxAngles;
    [SerializeField] Vector2 currentAngle;
    [SerializeField] Transform pointer;
    private void Start()
    {
        floorLayer = 1 << floorLayer;
    }
    void Update()
    {
        //just raycast only to the floor at all times to get the altitude
        Vector2 point = (Physics2D.Raycast(player.position, Vector2.down, Mathf.Infinity, floorLayer).point);
        height = Mathf.Floor(player.position.y - point.y);
        text.text = height.ToString();

        //set rotation of the pointer based off height / maxMeterHeight. If height > maxMeterHeight, just make the pointer stay at the max angle.
        // multiply the fraction by the different between the min and max angles and then add that value to the min value in order to get the right angle.
    }
}
