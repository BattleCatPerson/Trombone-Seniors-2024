using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxCharacterController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform sprite;
    void Start()
    {
        
    }

    void Update()
    {
        sprite.right = rb.velocity.normalized;
    }
}
