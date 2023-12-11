using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] Transform floor;
    [SerializeField] float floorAngle;
    [SerializeField] List<GameObject> ramps;
    [SerializeField] Transform playerTracker;
    [SerializeField] Transform player;

    private void Start()
    {
        //use renderer.isvisible;
        floorAngle = floor.eulerAngles.z;
        playerTracker.eulerAngles = Vector3.forward * floorAngle;
    }
    private void Update()
    {
        playerTracker.position = player.position;
    }
}
