using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    public GameObject flameThrower;

    private bool canFire = true;
    private bool isFiring;

    private void Start()
    {
        flameThrower.GetComponent<ParticleSystem>().Stop();
        flameThrower.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && canFire)
        {
            if (GameMaster.instance.currentWeapon == "Projectiles")
            {
                FireProjectile();
            }
            else if (GameMaster.instance.currentWeapon == "Flamethrower")
            {
                StartFlameThrower();
            }
        }

        if ((Input.GetMouseButtonUp(0) || GameMaster.instance.currentWeapon != "Flamethrower") && isFiring)
        {
            StopFlameThrower();
        }
    }

    private void FireProjectile()
    {
        canFire = false;

        Vector2 direction = transform.up; // Use the forward direction of the GameObject
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.rotation = transform.rotation;
        projectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
        projectile.layer = LayerMask.NameToLayer("Projectiles");


        StartCoroutine(ResetFireDelay());
    }

    private void StartFlameThrower()
    {
        isFiring = true;
        flameThrower.GetComponent<ParticleSystem>().Play();
        flameThrower.SetActive(true);
    }

    private void StopFlameThrower()
    {
        isFiring = false;
        flameThrower.GetComponent<ParticleSystem>().Stop();
        flameThrower.SetActive(false);
    }

    private IEnumerator ResetFireDelay()
    {
        yield return new WaitForSeconds(GameMaster.instance.projectileDelay);
        canFire = true;
    }
}
