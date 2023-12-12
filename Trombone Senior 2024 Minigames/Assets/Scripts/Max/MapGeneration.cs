using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class MapGeneration : MonoBehaviour
{
    [Serializable]
    public class ObjectActive
    {
        public Transform transform;
        public bool active;
        public static List<ObjectActive> activeObjects = new();
        public static List<ObjectActive> standbyObjects = new();
        public void MoveAway(Vector2 pos)
        {
            transform.position = pos;
            standbyObjects.Add(this);
            activeObjects.Remove(this);

            int index = Random.Range(0, standbyObjects.Count);
            activeObjects.Add(standbyObjects[index]);
            standbyObjects[index].MoveTo(Vector2.zero);
        }

        public void MoveTo(Vector2 pos)
        {
            //tbi
        }
    }

    [SerializeField] Transform floor;
    [SerializeField] List<ObjectActive> objects;
    [SerializeField] float floorAngle;
    [SerializeField] Transform playerTracker;
    [SerializeField] Transform player;
    [SerializeField] MaxCharacterController controller;
    [SerializeField] float distance;
    [SerializeField] Transform holdPoint;

    [SerializeField] List<ObjectActive> active;
    [SerializeField] List<ObjectActive> standby;
    private void Start()
    {
        //use renderer.isvisible;
        controller = player.GetComponent<MaxCharacterController>();
        floorAngle = floor.eulerAngles.z;

        foreach (ObjectActive o in objects) ObjectActive.standbyObjects.Add(o);
    }
    private void Update()
    {
        playerTracker.position = player.position;

        active = ObjectActive.activeObjects;
        standby = ObjectActive.standbyObjects;
        if (controller.Colliding)
        {
            if (active.Count > 0)
            {
                foreach (ObjectActive o in active)
                {
                    if (Mathf.Abs(o.transform.localPosition.x - playerTracker.localPosition.x) > distance)
                    {
                        o.MoveAway(holdPoint.position);
                    }
                }
            }
            else
            {
                // implement move to code here!
            }
            
        }
    }
}
