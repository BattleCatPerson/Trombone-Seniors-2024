using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Rigidbody2D rb;
    void Start()
    {

    }

    void Update()
    {
        transform.right = (target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target.position, rb.velocity.magnitude * Time.deltaTime * 2);
    }
}
