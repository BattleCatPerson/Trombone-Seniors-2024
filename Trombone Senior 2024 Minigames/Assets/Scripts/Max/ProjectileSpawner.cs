using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] float frequency;
    [SerializeField] float timer;
    [SerializeField] float speed;
    [SerializeField] float sideLength;
    [SerializeField] float minHeight;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] Transform projectilePrefab;
    [SerializeField] Transform spawnParent;
    [SerializeField] Rigidbody2D playerRb;
    //get bounds
    //get random direction
    void Update()
    {
        if (!MaxGameManager.started) return;
        if (timer <= 0)
        {
            Vector2 point = (Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, 1 << 8).point);
            float height = transform.position.y - point.y;
            if (height > minHeight)
            {
                SpawnProjectile();
                timer = frequency;
            }
        }
        else timer -= Time.deltaTime;
    }

    //use to spawn laser at the edge, then make it move into position!!!!
    public void SpawnProjectile()
    {
        int ind = Random.Range(0, sprites.Count);
        //assign sprite on projectile
        //spawn prefab at point on bounds
        //
        Vector2 point = new();
        int plane = Random.Range(0, 4);
        float randX = Random.Range(-sideLength, sideLength);
        float randY = Random.Range(-sideLength, sideLength);

        if (plane == 0 || plane == 1)
        {
            int mult = plane == 0 ? 1 : -1;
            point = new(mult * sideLength, randY);
        }
        else
        {
            int mult = plane == 2 ? 1 : -1;
            point = new(randX, mult * sideLength);
        }
        Vector2 offset = point;
        point += (Vector2)transform.position;

        Transform t = Instantiate(projectilePrefab, point, transform.rotation);
        t.parent = spawnParent;

        Projectile p = t.GetComponent<Projectile>();
        p.playerRb = playerRb;
        p.initialPosition = offset;

        float angle = Random.Range(0, 360f);
        t.eulerAngles = Vector3.forward * angle;
    }

}
