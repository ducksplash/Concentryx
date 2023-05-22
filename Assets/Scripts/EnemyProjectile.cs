using System.Collections.Generic;
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

        if (canFire)
        {
            FireProjectile();
            canFire = false;
        }
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate the direction from the projectile to the object being rotated around
        Vector3 direction = player.transform.position - projectile.transform.position;

        // Set the rotation of the projectile to face the object being rotated around
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        projectile.transform.rotation = rotation;

        // Set the velocity of the projectile to move towards the object being rotated around
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        projectileRigidbody.velocity = direction.normalized * projectileSpeed;
        projectile.layer = LayerMask.NameToLayer("Projectiles");
    }
}
