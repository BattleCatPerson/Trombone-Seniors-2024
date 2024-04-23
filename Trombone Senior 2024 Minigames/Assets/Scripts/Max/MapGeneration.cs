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
    [SerializeField] float floorLength;
    [SerializeField] float floorX;
    [SerializeField] List<ObjectActive> objects;
    [SerializeField] float floorAngle;
    [SerializeField] Transform playerTracker;
    [SerializeField] Transform player;
    [SerializeField] Vector3 initialPos;
    [SerializeField] Transform spriteTracker;
    [SerializeField] Transform sprite;

    [SerializeField] MaxCharacterController controller;
    public MaxCharacterController Controller => controller;
    [SerializeField] float distance;
    [SerializeField] Transform holdPoint;

    [SerializeField] List<ObjectActive> active;
    [SerializeField] List<ObjectActive> standby;

    [SerializeField] Transform currentFloor;
    [SerializeField] Transform standbyFloor;
    [SerializeField] Transform permanentFloor; //use this to create a floor at the end that travels all the way to the death point.

    [SerializeField] List<ObjectActive> skySprites;
    [SerializeField] float zOffset;
    private void Start()
    {
        //use renderer.isvisible;
        initialPos = currentFloor.position;
        floorAngle = floor.eulerAngles.z;
        ObjectActive.standbyObjects.Clear();
        ObjectActive.activeObjects.Clear();
        foreach (ObjectActive o in objects)
        {
            ObjectActive.standbyObjects.Add(o);
            o.Move(holdPoint.position);
        }
        floorX = floorLength * MathF.Cos(Mathf.Abs(floor.transform.eulerAngles.z) * MathF.PI / 180f);
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


        if (spriteTracker.position.x >= currentFloor.position.x + floorX / 2)
        {
            MoveFloors();
        }
    }

    public void MoveFloors()
    {
        Transform temp = currentFloor;
        standbyFloor.localPosition = new Vector3(temp.localPosition.x + floorLength, 0);
        currentFloor = standbyFloor;
        standbyFloor = temp;
    }

    public void SetPermaFloor() => permanentFloor.localScale = new Vector3(currentFloor.localPosition.x * 2, 1, 1);
    public void ResetPermaFloor() => permanentFloor.localScale = Vector3.one;

    public void Restart()
    {
        Debug.Log("Map gen restart");
        ObjectActive.standbyObjects.Clear();
        ObjectActive.activeObjects.Clear();
        foreach (ObjectActive o in objects)
        {
            ObjectActive.standbyObjects.Add(o);
            o.Move(holdPoint.position);
        }

        currentFloor.position = initialPos;
        standbyFloor.position = initialPos;
    }
}
