using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    public GameObject Player;

    private bool canFire = true;

    public float enemyProjectileDelay = 0.4f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (canFire)
        {
            StartCoroutine(FireProjectile());
        }
    }

    IEnumerator FireProjectile()
    {
        canFire = false;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate the direction from the projectile to the object being rotated around
        Vector3 direction = Player.transform.position - projectile.transform.position;

        // Set the rotation of the projectile to face the object being rotated around
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        projectile.transform.rotation = rotation;

        // Set the velocity of the projectile to move towards the object being rotated around
        projectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
        projectile.layer = LayerMask.NameToLayer("Projectiles");

        yield return new WaitForSeconds(enemyProjectileDelay);

        canFire = true;
    }
}
