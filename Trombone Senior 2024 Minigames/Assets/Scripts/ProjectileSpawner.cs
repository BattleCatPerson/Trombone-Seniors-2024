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
    //get bounds
    //get random direction
    void Update()
    {
        if (timer <= 0)
        {
            Vector2 point = (Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, 1 << 8).point);
            float height = transform.position.y - point.y;
            if (height > minHeight)
            {
                SpawnAttack();
                timer = frequency;
            }
        }
        else timer -= Time.deltaTime;
    }

    //public void SpawnProjectile()
    //{
    //    int ind = Random.Range(0, sprites.Count);
    //    //assign sprite on projectile
    //    //spawn prefab at point on bounds
    //    //
    //    Vector2 point = new();
    //    int plane = Random.Range(0, 4);
    //    float randX = Random.Range(-sideLength, sideLength);
    //    float randY = Random.Range(-sideLength, sideLength);

    //    if (plane == 0 || plane == 1)
    //    {
    //        int mult = plane == 0 ? 1 : -1;
    //        point = new(mult * sideLength, randY);
    //    }
    //    else
    //    {
    //        int mult = plane == 2 ? 1 : -1;
    //        point = new(randX, mult * sideLength);
    //    }

    //    point += (Vector2)transform.position;

    //    Projectile p = Instantiate(projectilePrefab, point, transform.rotation);
    //    float angle = Random.Range(0, 2 * Mathf.PI);
    //    Vector2 direction = (new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))).normalized;
    //    //Vector2 direction = ((Vector2)transform.position - point).normalized;

    //    Sprite sprite = sprites[Random.Range(0, sprites.Count)];
    //    p.Assign(direction * speed, sprite);
    //}
    public void SpawnAttack()
    {
        float angle = Random.Range(0, 360f);

        Transform t = Instantiate(projectilePrefab, transform.position, transform.rotation);
        t.parent = spawnParent;
        t.eulerAngles = Vector3.forward * angle;
    }

}
