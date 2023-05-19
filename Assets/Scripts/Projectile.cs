using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    public GameObject flameThrower;

    public bool canFire = true;

    void Start()
    {
        flameThrower.GetComponent<ParticleSystem>().Stop();
        flameThrower.SetActive(false);
    }


    void Update()
    {
        if (Input.GetMouseButton(0) && canFire)
        {

            if (GameMaster.instance.currentWeapon == "Projectiles")
            {
                StartCoroutine(FireProjectile());
            }

            if (GameMaster.instance.currentWeapon == "Flamethrower")
            {
                flameThrower.GetComponent<ParticleSystem>().Play();
                flameThrower.SetActive(true);
                Debug.Log("Flamethrower pew");
            }
        }

        if (Input.GetMouseButtonUp(0) || GameMaster.instance.currentWeapon != "Flamethrower")
        {

            flameThrower.GetComponent<ParticleSystem>().Stop();
            flameThrower.SetActive(false);

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
