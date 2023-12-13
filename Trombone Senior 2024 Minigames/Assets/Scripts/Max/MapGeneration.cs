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
        public void MoveAway(Vector2 pos) => transform.position = pos;

        public void MoveTo(Vector2 pos) => transform.localPosition = pos;

        public static void SelectNew(Vector2 pos)
        {
            ObjectActive newObj = standbyObjects[Random.Range(0, standbyObjects.Count)];
            standbyObjects.Remove(newObj);
            activeObjects.Add(newObj);

            newObj.MoveTo(pos);
        }

        public static void Remove(ObjectActive o, Vector2 pos)
        {
            standbyObjects.Add(o);
            activeObjects.Remove(o);

            o.MoveAway(pos);
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

        foreach (ObjectActive o in objects)
        {
            ObjectActive.standbyObjects.Add(o);
            o.MoveAway(holdPoint.position);
        }
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
                        ObjectActive.Remove(o, holdPoint.position);
                        break;
                    }
                }
            }
            else
            {
                ObjectActive.SelectNew(playerTracker.localPosition + Vector3.right * distance);
            }

        }
    }
}
