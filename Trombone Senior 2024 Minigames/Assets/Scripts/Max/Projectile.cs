using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;

    public void Assign(Vector2 velocity, Sprite sprite)
    {
        rb.velocity = velocity;
        spriteRenderer.sprite = sprite;
    }
}
