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
    [SerializeField] Transform cannon;
    [SerializeField] LayerMask mask;
    [SerializeField] bool shot;
    [SerializeField] SpriteRenderer countdownRenderer;
    [SerializeField] List<Sprite> countdownSprites;
    [SerializeField] float offset;
    [SerializeField] Transform laserPoint;
    SpriteRenderer renderer;
    private void Start()
    {
        //Destroy(gameObject, lifeTime);
        renderer = laser.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        countdownRenderer.sprite = countdownSprites[0];
        laserPoint.gameObject.SetActive(false);
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
            RaycastHit2D p = Physics2D.Raycast(transform.position + transform.right * offset, -cannon.right, Mathf.Infinity, mask);
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
            laserPoint.gameObject.SetActive(true);
            laserPoint.position = point;
            Debug.Log(point);
            float d = Vector2.Distance(point, transform.position + transform.right * offset);
            laser.localPosition = Vector2.right * offset - Vector2.right * d / 2;
            laser.localScale = new(d, laser.localScale.y);
            Destroy(gameObject, 2f);

            Debug.Log(d);

        }

    }
}
