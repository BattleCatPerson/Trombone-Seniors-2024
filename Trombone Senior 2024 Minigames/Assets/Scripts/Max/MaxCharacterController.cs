using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxCharacterController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform sprite;
    [SerializeField] Vector2 direction;
    [SerializeField] float speed;
    void Start()
    {
        
    }

    void Update()
    {
        sprite.position = rb.position;
        sprite.right = direction;
        if (direction != Vector2.zero) rb.velocity = direction.normalized * speed;
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
            int previous = Mathf.Clamp(index - 1, 0, e.pointCount - 1);
            Vector2 slope = e.points[next] - e.points[previous];
            direction = slope;
        }
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
