using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    [SerializeField] Vector3 limit;
    [SerializeField] Transform resetPostion;
    [SerializeField] Transform tracker;
    [SerializeField] List<Transform> affectedTransforms;
    [SerializeField] Altimeter altimeter;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tracker.position.x > limit.x || tracker.position.y > limit.y)
        {
            Vector3 newPos = resetPostion.position;
            foreach (Transform t in affectedTransforms)
            {
                Vector3 offset = t.position - tracker.position;
                t.position = newPos + offset;
            }
            tracker.position = newPos;
        }
    }
}
