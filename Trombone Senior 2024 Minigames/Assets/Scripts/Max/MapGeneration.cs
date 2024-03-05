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
        public static List<ObjectActive> activeObjects = new();
        public static List<ObjectActive> standbyObjects = new();
        public void Move(Vector3 pos)
        {
            transform.localPosition = (Vector2)pos;
            transform.position += Vector3.forward * pos.z;
        }
        public static void SelectNew(Vector3 pos)
        {
            ObjectActive newObj = standbyObjects[Random.Range(0, standbyObjects.Count)];
            standbyObjects.Remove(newObj);
            activeObjects.Add(newObj);

            newObj.Move(pos);
        }

        public static void Remove(ObjectActive o, Vector2 pos)
        {
            standbyObjects.Add(o);
            activeObjects.Remove(o);

            o.Move(pos);
        }
    }

    [SerializeField] Transform floor;
    [SerializeField] List<ObjectActive> objects;
    [SerializeField] float floorAngle;
    [SerializeField] Transform playerTracker;
    [SerializeField] Transform player;

    [SerializeField] Transform spriteTracker;
    [SerializeField] Transform sprite;

    [SerializeField] MaxCharacterController controller;
    [SerializeField] float distance;
    [SerializeField] Transform holdPoint;

    [SerializeField] List<ObjectActive> active;
    [SerializeField] List<ObjectActive> standby;

    [SerializeField] Transform currentFloor;
    [SerializeField] Transform standbyFloor;

    [SerializeField] List<ObjectActive> skySprites;
    [SerializeField] float zOffset;

    [SerializeField] Transform altitudeCollider;
    private void Start()
    {
        //use renderer.isvisible;
        floorAngle = floor.eulerAngles.z;
        ObjectActive.standbyObjects.Clear();
        ObjectActive.activeObjects.Clear();
        foreach (ObjectActive o in objects)
        {
            ObjectActive.standbyObjects.Add(o);
            o.Move(holdPoint.position);
        }
    }
    private void Update()
    {
        playerTracker.position = player.position;
        spriteTracker.position = sprite.position;

        active = ObjectActive.activeObjects;
        standby = ObjectActive.standbyObjects;
        if (controller.Colliding)
        {
            if (active.Count > 0)
            {
                foreach (ObjectActive o in active)
                {
                    if (Mathf.Abs(o.transform.localPosition.x - spriteTracker.localPosition.x) > distance)
                    {
                        ObjectActive.Remove(o, holdPoint.position);
                        ObjectActive.SelectNew(Vector3.right * (distance + playerTracker.localPosition.x) + Vector3.forward * zOffset);
                        break;
                    }
                }
            }
            else
            {
                ObjectActive.SelectNew(Vector3.right * (distance + playerTracker.localPosition.x) + Vector3.forward * zOffset);
            }
        }


        if (spriteTracker.localPosition.x >= currentFloor.localPosition.x - 10f)
        {
            MoveFloors();
        }

        altitudeCollider.localPosition = Vector3.right * playerTracker.localPosition.x; 
    }

    public void MoveFloors()
    {
        Transform temp = currentFloor;
        standbyFloor.localPosition = Vector3.right * (currentFloor.localPosition.x + currentFloor.lossyScale.x);
        currentFloor = standbyFloor;
        standbyFloor = temp;
    }
}
