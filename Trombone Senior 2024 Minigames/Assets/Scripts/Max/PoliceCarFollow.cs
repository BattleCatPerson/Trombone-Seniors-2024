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
    private bool started = false;
    public void Enable() => started = true;

    void FixedUpdate()
    {
        if (!started) return;
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, smoothTime);
    }
}
