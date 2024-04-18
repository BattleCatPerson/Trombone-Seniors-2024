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
    void Update()
    {
        if (tracker.position.x > limit.x)
        {
            Vector3 newPos = resetPostion.position + Vector3.up * altimeter.floatHeight;
            foreach (Transform t in affectedTransforms)
            {
                Vector3 offset = t.position - tracker.position;
                t.position = newPos + offset;
            }
            tracker.position = newPos;
        }
    }
}
