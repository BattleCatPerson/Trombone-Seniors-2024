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
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        sprite.position = rb.position;
        //sprite.right = direction;
        if (direction != Vector2.zero)
        {
            rb.velocity = direction.normalized * speed;
            var activeTouches = Touch.activeTouches;

            if (activeTouches.Count > 0) speed += increaseRate * Time.fixedDeltaTime;
            speed = Mathf.Clamp(speed, 0, speedCap);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.TryGetComponent<EdgeCollider2D>(out EdgeCollider2D e))
        {
            //foreach (ContactPoint2D c in collision.contacts)
            //{
            //    Debug.Log(c.point);
            //}

            int index = ReturnClosestPoint(e, collision.GetContact(collision.contactCount - 1).point);
            int next = Mathf.Clamp(index + 1, 0, e.pointCount - 1);
            //int previous = Mathf.Clamp(index - 1, 0, e.pointCount - 1);
            //Vector2 slope = e.points[next] - e.points[previous];
            //direction = slope;
            
            if (next != index)
            {
                direction = e.points[next] - e.points[index];
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        direction = Vector2.zero;
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
