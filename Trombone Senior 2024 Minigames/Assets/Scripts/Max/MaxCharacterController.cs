using System;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Random = UnityEngine.Random;
using Mathf = UnityEngine.Mathf;
using Cinemachine;
using UnityEngine.UI;
public class MaxCharacterController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform sprite;
    [SerializeField] Vector2 direction;
    [SerializeField] float speed;
    [SerializeField] float downwardForceRate;
    [SerializeField] bool colliding;
    [SerializeField] int collidersTouching;
    [SerializeField] Slider slider;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] float verticalDistanceToFloor;
    [SerializeField] int layer;
    [SerializeField] float baseCameraSize;
    void Start()
    {
        layer = 1 << layer;
    }

    void Update()
    {
        sprite.position = rb.position;
        var activeTouches = Touch.activeTouches;
        if (direction.magnitude > 0)
        {
            rb.velocity = (Vector2)direction * speed;
            transform.right = direction;
        }
        if (!colliding)
        {
            if (slider.value > 0) rotationSpeed += acceleration * slider.value * Time.deltaTime;
            else rotationSpeed += deceleration * slider.value * Time.deltaTime;
            rotationSpeed = Mathf.Clamp(rotationSpeed, 0, maxRotationSpeed);
            transform.eulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }

        Vector2 point = (Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layer).point);
        verticalDistanceToFloor = Vector2.Distance((Vector2) transform.position + Vector2.down * transform.localScale, point);

        if (verticalDistanceToFloor > 1f)
        {
            vCam.m_Lens.OrthographicSize = baseCameraSize + verticalDistanceToFloor;
        }
        else vCam.m_Lens.OrthographicSize = baseCameraSize;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //float angle = Vector2.Angle(collision.GetContact(0).normal, collision.relativeVelocity);
        //if (collision.GetContact(0).normal.x < 0)
        //{
        //    float sum = 0;
        //    foreach (var c in collision.contacts)
        //    {
        //        Debug.Log(c.normalImpulse);
        //        sum += c.normalImpulse;
        //    }
        //    if (sum / collision.contactCount / rb.mass > impactLimit)
        //    {
        //        Debug.Log("DIE");
        //        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //    }
        //}
        //sprite.up = collision.GetContact(0).normal;

        colliding = true;
        collidersTouching++;
        Vector2 v = Vector2.Perpendicular(-collision.GetContact(0).normal);
        rb.velocity = v * rb.velocity.magnitude;
        direction = v;
        rb.gravityScale = 0;

        rotationSpeed = 0;
    }
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    colliding = true;
    //    sprite.up = collision.GetContact(0).normal;
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        collidersTouching--;
        if (collidersTouching == 0)
        {
            colliding = false;
            rb.gravityScale = 0.5f;
            direction = Vector2.zero;
        }
    }


}
