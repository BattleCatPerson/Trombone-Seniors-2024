using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //[SerializeField] Rigidbody2D rb;
    //[SerializeField] SpriteRenderer spriteRenderer;
    //[SerializeField] float lifeTime;
    [SerializeField] float time;
    [SerializeField] Transform child;
    [SerializeField] LayerMask mask;
    private void Start()
    {
        //Destroy(gameObject, lifeTime);
    }
    //public void Assign(Vector2 velocity, Sprite sprite)
    //{
    //    rb.velocity = velocity;
    //    spriteRenderer.sprite = sprite;
    //}
    private void Update()
    {
        if (time >= 0) time -= Time.deltaTime;
        else
        {
            RaycastHit2D p = Physics2D.Raycast(child.position, -child.right, Mathf.Infinity, mask);
            if (!p.collider) return;

            if (LayerMask.LayerToName(p.collider.gameObject.layer) == "Player")
            {
                Debug.Log("DIE");
            }
            else
            {
                Debug.Log("BLOCK");
            }

            Destroy(gameObject);
        }
            
    }
}
