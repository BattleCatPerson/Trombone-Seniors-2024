using System;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Random = UnityEngine.Random;

public class MaxCharacterController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform sprite;
    [SerializeField] Vector2 direction;
    [SerializeField] float speed;
    [SerializeField] float increaseRate;
    [SerializeField] float speedCap;
    [SerializeField] float downwardForceRate;

    [SerializeField] bool colliding;
    [SerializeField] float impactLimit;
    void Start()
    {

    }

    void Update()
    {
        sprite.position = rb.position;
        var activeTouches = Touch.activeTouches;
        if (colliding && activeTouches.Count > 0)
        {
            rb.angularVelocity += -speed * Time.deltaTime;
        }
        else if (activeTouches.Count > 0) rb.velocity += Vector2.down * downwardForceRate * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float angle = Vector2.Angle(collision.GetContact(0).normal, collision.relativeVelocity);
        if (collision.GetContact(0).normal.x < 0)
        {
            float sum = 0;
            foreach (var c in collision.contacts)
            {
                Debug.Log(c.normalImpulse);
                sum += c.normalImpulse;
            }
            if (sum / collision.contactCount / rb.mass > impactLimit)
            {
                Debug.Log("DIE");
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        sprite.up = collision.GetContact(0).normal;
        colliding = true;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        colliding = true;
        sprite.up = collision.GetContact(0).normal;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        direction = Vector2.zero;
        colliding = false;
    }

    public int ReturnClosestPoint(EdgeCollider2D e, Vector2 position)
    {
        float minDistance = Mathf.Infinity;
        int index = -1;
        for (int i = 0; i < e.pointCount; i++)
        {
            Vector2 v = e.points[i];
            float distance = Vector2.Distance(position, v);
            if (distance < minDistance)
            {
                index = i;
                minDistance = distance;
            }
        }

        return index;
    }

}
