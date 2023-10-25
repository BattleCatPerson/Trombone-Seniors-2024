using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] Transform target;
    [SerializeField] bool grabbed;
    [SerializeField] Rigidbody2D rb;
    void Start()
    {
        
    }

    void Update()
    {
        rb.velocity = grabbed ? Vector3.zero : (target.position - transform.position).normalized * speed;
    }

    public void Grab(bool b) => grabbed = b;

    public void SetTarget(Transform t) => target = t;
}
