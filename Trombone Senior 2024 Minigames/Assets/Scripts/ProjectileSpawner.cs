using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] float frequency;
    [SerializeField] float timer;
    [SerializeField] float speed;
    [SerializeField] float sideLength;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] Projectile projectilePrefab;
    //get bounds
    //get random direction
    void Start()
    {
        
    }

    void Update()
    {
        if (timer <= 0)
        {
            SpawnProjectile();
            timer = frequency;
        }
        else timer -= Time.deltaTime; 
    }

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

        point += (Vector2)transform.position;

        Projectile p = Instantiate(projectilePrefab, point, transform.rotation);
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 direction = (new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))).normalized;

        Sprite sprite = sprites[Random.Range(0, sprites.Count)];
        p.Assign(direction * speed, sprite);
    }

}
