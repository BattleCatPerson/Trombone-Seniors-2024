using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    [SerializeField] Vector3 limit;
    [SerializeField] Transform resetPostion;
    [SerializeField] Transform tracker;
    [SerializeField] List<Transform> affectedTransforms;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tracker.position.x > limit.x || tracker.position.y > limit.y)
        {
            foreach (Transform t in affectedTransforms)
            {
                Vector3 offset = t.position - tracker.position;
                t.position = resetPostion.position + offset;
            }
            tracker.position = resetPostion.position;
        }
    }
}
