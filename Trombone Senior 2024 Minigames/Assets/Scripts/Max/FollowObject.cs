using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 offsetOnScreen;
    [SerializeField] Transform target;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] int layer;
    [SerializeField] Transform sprite;
    [SerializeField] Vector2 point;
    public bool spriteFollowActive;
    [SerializeField] float transitionSpeed;
    [SerializeField] bool moving;
    [SerializeField] Sprite initialSprite;
    [SerializeField] Sprite sickSprite;
    [SerializeField] SpriteRenderer renderer;
    private void Start()
    {
        layer = 1 << layer;
        rb = target.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        //raycast ground, make y level yOffset from that raycast point! eASTY!
        transform.position = target.position + Vector3.right * offset.x + Vector3.up * 10f;
        point = (Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layer).point);
        transitionSpeed = rb.velocity.magnitude * 4;
        if (spriteFollowActive)
        {
            Vector2 newPoint = point + Vector2.up * offset.y;
            if (Vector3.Distance(sprite.position, newPoint) > 0.5f && moving) sprite.position = Vector3.MoveTowards(sprite.position, newPoint, transitionSpeed * Time.fixedDeltaTime);
            else
            {
                sprite.position = newPoint;
                moving = false;
            }
        }
        else
        {
            Vector2 newPoint = (Vector2)target.position + offsetOnScreen;

            if (Vector3.Distance(sprite.position, newPoint) > 0.5f && moving) sprite.position = Vector3.MoveTowards(sprite.position, newPoint, transitionSpeed * Time.fixedDeltaTime);
            else
            {
                sprite.position = newPoint;
                moving = false;
            }
        }
        //when you hit "sick" change sprite of the sun and lerp to the offsetOnScreen vector
        //when you die/land, change back and lerp away
    }

    public void SetFollowActive()
    {
        spriteFollowActive = true;
        moving = true;
        renderer.sprite = initialSprite;
    }

    public void SetFollowDisable()
    {
        spriteFollowActive = false;
        moving = true;
        renderer.sprite = sickSprite;
    }
}
