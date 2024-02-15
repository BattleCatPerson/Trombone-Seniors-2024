using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;
    [SerializeField] Vector3 offset;
    [SerializeField] List<Vector3> positions;
    [SerializeField] int index;
    void Start()
    {
        positions.Add(target.position + offset);
    }

    void Update()
    {
        positions.Add(target.position + offset);
        transform.right = (target.position - transform.position).normalized;
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, smoothTime);
        //transform.position = positions[index];
        //index++;
    }
}
