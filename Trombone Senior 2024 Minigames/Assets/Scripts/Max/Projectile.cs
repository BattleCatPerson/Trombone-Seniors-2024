using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //[SerializeField] Rigidbody2D rb;
    //[SerializeField] SpriteRenderer spriteRenderer;
    //[SerializeField] float lifeTime;
    const float MULT = 0.9f;
    public static float time = 3f;
    public static float currentTime = time;
    [SerializeField] float accumulated;
    [SerializeField] Transform laser;
    [SerializeField] Transform cannon;
    [SerializeField] LayerMask mask;
    [SerializeField] bool shot;
    [SerializeField] SpriteRenderer countdownRenderer;
    [SerializeField] List<Sprite> countdownSprites;
    [SerializeField] float offset;
    [SerializeField] Transform laserPoint;
    [SerializeField] float duration;
    public Vector2 initialPosition;
    [SerializeField] bool moving;
    [SerializeField] float moveSpeed;
    public Vector3 position;
    public Rigidbody2D playerRb;
    SpriteRenderer renderer;
    private void Start()
    {
        //Destroy(gameObject, lifeTime);
        moving = true;
        renderer = laser.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        countdownRenderer.sprite = countdownSprites[0];
        laserPoint.gameObject.SetActive(false);
        moveSpeed = playerRb.velocity.magnitude;
    }
    //public void Assign(Vector2 velocity, Sprite sprite)
    //{
    //    rb.velocity = velocity;
    //    spriteRenderer.sprite = sprite;
    //}
    private void Update()
    {
        if (MaxGameManager.gameOver) return;
        transform.right = (playerRb.position - (Vector2)transform.position).normalized;
        if (moving)
        {
            if (!shot)
            {
                moveSpeed += (moveSpeed * 1.1f * Time.deltaTime);
                Vector2 newPos = playerRb.transform.position + position * offset;
                if (Vector2.Distance(transform.position, newPos) > 0.5) transform.position = Vector2.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
                else
                {
                    transform.position = newPos;
                    moving = false;
                }
            }
            else
            {
                Vector2 newPos = (Vector2) playerRb.transform.position + initialPosition;
                if (Vector2.Distance(transform.position, newPos) > 0.5) transform.position = Vector2.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
                else
                {
                    Destroy(gameObject);
                }
            }
            return;
        }

        if (shot && duration >= 0)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                moving = true;
                renderer.enabled = false;
                laserPoint.gameObject.SetActive(false);
            }
        }

        if (accumulated < currentTime)
        {
            accumulated += Time.deltaTime;
            int ind = 0;
            if (accumulated >= 2 * time / 3) ind = 2;
            else if (accumulated >= time / 3) ind = 1;

            countdownRenderer.sprite = countdownSprites[ind];

        }
        else if (!shot)
        {
            RaycastHit2D p = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, mask);
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
            float d = Vector2.Distance(point, transform.position);
            laser.position = transform.position + transform.right * d / 2;
            laser.localScale = new(d, laser.localScale.y);
            //Destroy(gameObject, 2f);

            Debug.Log(d);

        }

    }

    public static void UpdateTime(int thousands)
    {
        currentTime = time * Mathf.Pow(MULT, thousands);
    }
}
