using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Altimeter : MonoBehaviour
{
    [SerializeField] float height;
    [SerializeField] int floorLayer;
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI text;
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
    }
}
