using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSetVelocity : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public Vector3 moveSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetVelocity()
    {
        GetComponent<Animator>().enabled = false;
        Debug.Log("Go");
        rb.velocity = moveSpeed; 
    }
}
