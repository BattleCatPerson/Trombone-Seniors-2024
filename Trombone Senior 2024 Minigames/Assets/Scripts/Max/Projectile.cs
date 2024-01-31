using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //[SerializeField] Rigidbody2D rb;
    //[SerializeField] SpriteRenderer spriteRenderer;
    //[SerializeField] float lifeTime;
    [SerializeField] float time;
    [SerializeField] float accumulated;
    [SerializeField] int currentInt;
    [SerializeField] Transform laser;
    [SerializeField] LayerMask mask;
    [SerializeField] bool shot;
    [SerializeField] SpriteRenderer countdownRenderer;
    [SerializeField] List<Sprite> countdownSprites;
    SpriteRenderer renderer;
    private void Start()
    {
        //Destroy(gameObject, lifeTime);
        renderer = laser.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        countdownRenderer.sprite = countdownSprites[0];
    }
    //public void Assign(Vector2 velocity, Sprite sprite)
    //{
    //    rb.velocity = velocity;
    //    spriteRenderer.sprite = sprite;
    //}
    private void Update()
    {
        if (accumulated < time)
        {
            accumulated += Time.deltaTime;
            if (accumulated > currentInt + 1 && currentInt != countdownSprites.Count - 1)
            {
                currentInt++;
                countdownRenderer.sprite = countdownSprites[currentInt];
            }
        }
        else if (!shot)
        {
            RaycastHit2D p = Physics2D.Raycast(laser.position, -laser.right, Mathf.Infinity, mask);
            if (!p.collider) return;

            if (LayerMask.LayerToName(p.collider.gameObject.layer) == "Player")
            {
                Debug.Log("DIE");
            }
            else
            {
                Debug.Log("BLOCK");
            }
            countdownRenderer.enabled = false;
            renderer.enabled = true;

            shot = true;
            Vector2 point = p.point;
            float d = Vector2.Distance(point, laser.position);
            laser.localPosition = (Vector2)laser.localPosition - Vector2.right * d / 2;
            renderer.size = new(renderer.size.x * d, renderer.size.y);
            Destroy(gameObject, 2f);

        }

    }
}
