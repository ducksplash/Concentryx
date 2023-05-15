using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    private bool canFire = true;

    void Update()
    {
        if (Input.GetMouseButton(0) && canFire)
        {
            StartCoroutine(FireProjectile());
        }
    }

    IEnumerator FireProjectile()
    {
        canFire = false;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.rotation = transform.rotation;
        projectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
        projectile.layer = LayerMask.NameToLayer("Projectiles");

        yield return new WaitForSeconds(GameMaster.instance.projectileDelay);

        canFire = true;
    }


}
