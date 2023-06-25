using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public GameObject player;

    private bool canFire = true;
    public float enemyProjectileDelay = 0.4f;
    private float fireTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fireTimer = enemyProjectileDelay;
    }

    private void Update()
    {
        if (!canFire)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                canFire = true;
                fireTimer = enemyProjectileDelay;
            }
        }
        else
        {
            FireProjectile();
            canFire = false;
        }
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector3 direction = player.transform.position - projectile.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        projectile.transform.rotation = rotation;

        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        projectileRigidbody.velocity = direction.normalized * projectileSpeed;
        projectile.layer = LayerMask.NameToLayer("Projectiles");
    }

}
