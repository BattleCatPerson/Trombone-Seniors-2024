using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //[SerializeField] Rigidbody2D rb;
    //[SerializeField] SpriteRenderer spriteRenderer;
    //[SerializeField] float lifeTime;
    const float MULT = 0.9f;
    public static float time = 3f;
    public static float currentTime = -1f;
    public static int projectileAmount = 0;
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
    public MaxCharacterController controller;
    SpriteRenderer renderer;
    private void Start()
    {
        //Destroy(gameObject, lifeTime);
        if (projectileAmount == 0) MaxGameManager.instance.ShieldActive(true);
        projectileAmount++;
        if (currentTime == -1) currentTime = time;
        moving = true;
        renderer = laser.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        countdownRenderer.sprite = countdownSprites[0];
        laserPoint.gameObject.SetActive(false);
        moveSpeed = playerRb.velocity.magnitude;

        MaxGameManager.instance.restartEvent.AddListener(DestroyOnRestart);
    }
    //public void Assign(Vector2 velocity, Sprite sprite)
    //{
    //    rb.velocity = velocity;
    //    spriteRenderer.sprite = sprite;
    //}
    private void FixedUpdate()
    { 
        if (MaxGameManager.gameOver) return;
        if (moving)
        {
            if (!shot)
            {
                moveSpeed += (moveSpeed * 1.1f * Time.fixedDeltaTime);
                Vector2 newPos = playerRb.transform.position + position * offset;
                if (Vector2.Distance(transform.position, newPos) > 0.5) transform.position = Vector2.MoveTowards(transform.position, newPos, moveSpeed * Time.fixedDeltaTime);
                else
                {
                    transform.position = newPos;
                    transform.right = (playerRb.position - (Vector2)transform.position).normalized;
                    moving = false;
                }
            }
            else
            {
                Vector2 newPos = (Vector2) playerRb.transform.position + initialPosition;
                if (Vector2.Distance(transform.position, newPos) > 0.5) transform.position = Vector2.MoveTowards(transform.position, newPos, moveSpeed * Time.fixedDeltaTime);
                else
                {
                    MaxGameManager.instance.restartEvent.RemoveListener(DestroyOnRestart);
                    DestroyOnRestart();
                }
            }
            return;
        }

        if (shot && duration >= 0)
        {
            duration -= Time.fixedDeltaTime;
            if (duration <= 0)
            {
                moving = true;
                renderer.enabled = false;
                laserPoint.gameObject.SetActive(false);
            }
        }

        if (accumulated < currentTime)
        {
            accumulated += Time.fixedDeltaTime;
            int ind = 0;
            if (accumulated >= 2 * currentTime / 3) ind = 2;
            else if (accumulated >= currentTime / 3) ind = 1;

            countdownRenderer.sprite = countdownSprites[ind];

        }
        else if (!shot)
        {
            Physics2D.SyncTransforms();
            RaycastHit2D p = Physics2D.Raycast(transform.position, transform.right, offset, mask);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, offset, mask);
            countdownRenderer.enabled = false;
            renderer.enabled = true;
            shot = true;
            //if (!p.collider) return;
            //if (LayerMask.LayerToName(p.collider.gameObject.layer) == "Player")
            //{
            //    Debug.Log("DIE");
            //    p.collider.transform.root.GetComponent<MaxCharacterController>().StopGame();
            //}
            Vector2 point = new Vector2();

            if (!p.collider)
            {
                Debug.Log("DIE");
                controller.StopGame();
                point = controller.transform.position;
            }
            else
            {
                point = p.point;
                Debug.Log("BLOCK");
            }
            
            laserPoint.gameObject.SetActive(true);
            laserPoint.position = point;
            Debug.Log(point);
            float d = Vector2.Distance(point, transform.position);
            laser.position = transform.position + transform.right * d / 2;
            laser.localScale = new(d, laser.localScale.y);
            //Destroy(gameObject, 2f);

            Debug.Log(d);

        }

        RaycastHit2D[] p1 = Physics2D.RaycastAll(transform.position, transform.right, offset, mask);
        Debug.Log($"DEBU G COUNT {p1.Length}");
    }

    public static void UpdateTime(int thousands)
    {
        currentTime = time * Mathf.Pow(MULT, thousands);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.right * offset);
    }

    public void DestroyOnRestart()
    {
        projectileAmount--;
        if (projectileAmount == 0) MaxGameManager.instance.ShieldActive(false);
        Destroy(gameObject);
    }
}
