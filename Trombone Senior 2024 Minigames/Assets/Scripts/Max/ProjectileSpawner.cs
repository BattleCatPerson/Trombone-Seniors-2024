using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] float frequency;
    [SerializeField] float minFrequency;
    private float baseFrequency;
    [SerializeField] float timer;
    [SerializeField] float speed;
    [SerializeField] float sideLength;
    [SerializeField] float minHeight;
    [SerializeField] Animator anim;
    [SerializeField] Transform projectilePrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Transform spawnParent;
    [SerializeField] Rigidbody2D playerRb;
    //[SerializeField] RuntimeAnimatorController droneAnimator;
    //[SerializeField] Material laserMaterial;
    [SerializeField] PoliceCarFollow.AnimatorToSprite animToSprite;
    [SerializeField] SpriteRenderer deployer;
    [SerializeField] SpriteRenderer deployerCannon;
    [SerializeField] MaxCharacterController controller;
    [SerializeField] float fractionFrequencyAfterThousand;
    [SerializeField] bool canShoot;
    [SerializeField] Shield shield;
    public bool outline;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material outlineMaterial;
    //get bounds
    //get random direction
    private void Start()
    {
        Projectile.projectileAmount = 0;
        baseFrequency = frequency;
    }
    void Update()
    {
        if (!MaxGameManager.started || MaxGameManager.gameOver) return;
        if (timer <= 0 && canShoot && !shield.Shooting)
        {
            Vector2 point = (Physics2D.Raycast(playerRb.transform.position, Vector2.down, Mathf.Infinity, 1 << 8).point);
            float height = transform.position.y - point.y;
            if (height > minHeight)
            {
                anim.SetTrigger("Spawn");
                timer = frequency;
            }
        }
        else timer -= Time.deltaTime;
    }

    //use to spawn laser at the edge, then make it move into position!!!!
    public void SpawnProjectile()
    {
        Vector2 offset = new();
        int plane = Random.Range(0, 4);
        float randX = Random.Range(-sideLength, sideLength);
        float randY = Random.Range(-sideLength, sideLength);

        if (plane == 0 || plane == 1)
        {
            int mult = plane == 0 ? 1 : -1;
            offset = new(mult * sideLength, randY);
        }
        else
        {
            int mult = plane == 2 ? 1 : -1;
            offset = new(randX, mult * sideLength);
        }

        Transform t = Instantiate(projectilePrefab, spawnPosition.position, transform.rotation);
        t.parent = spawnParent;

        Projectile p = t.GetComponent<Projectile>();
        p.playerRb = playerRb;
        p.initialPosition = offset;
        p.Assign(animToSprite);

        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        p.position = position;

        p.controller = controller;

        if (outline) p.droneRenderer.material = outlineMaterial;
        else p.droneRenderer.material = defaultMaterial;
    }

    public void UpdateFrequency(int thousands)
    {
        frequency = baseFrequency * Mathf.Pow(fractionFrequencyAfterThousand, thousands);
        frequency = Mathf.Clamp(frequency, minFrequency, frequency);
    }

    public void DisableShooting() => canShoot = false;
    public void EnableShooting() => canShoot = true;
    public void Assign(PoliceCarFollow.AnimatorToSprite a)
    {
        animToSprite = a;
        deployer.color = a.deployerColor;
        deployerCannon.sprite = a.deployerSprite;
    }
}
