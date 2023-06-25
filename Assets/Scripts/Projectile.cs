using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    public GameObject flameThrower;

    private bool canFire = true;
    private bool isFiring;

    private int projectileCounter = 0;

    private AudioSource audioSource;
    private Coroutine firingCoroutine;

    private void Start()
    {
        flameThrower.GetComponent<ParticleSystem>().Stop();
        flameThrower.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && canFire)
        {
            FireSomething();
        }

        if ((Input.GetMouseButtonUp(0) || GameMaster.instance.currentWeapon != "Flamethrower") && isFiring)
            StopFlameThrower();
    }


    private void FireSomething()
    {
        if (GameMaster.instance.currentWeapon == "Projectiles")
            FireProjectile();
        else if (GameMaster.instance.currentWeapon == "Flamethrower")
            StartFlameThrower();
    }


    private void FireProjectile()
    {
        canFire = false;

        if (firingCoroutine == null)
            firingCoroutine = StartCoroutine(StartAudio());

        Vector2 direction = transform.up; // Use the forward direction of the GameObject
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
        projectile.layer = LayerMask.NameToLayer("Projectiles");

        projectileCounter++;

        StartCoroutine(ResetFireDelay());
    }

    private void StartFlameThrower()
    {
        audioSource.Play();
        canFire = false;
        isFiring = true;
        flameThrower.GetComponent<ParticleSystem>().Play();
        flameThrower.SetActive(true);
    }

    private void StopFlameThrower()
    {
        isFiring = false;
        canFire = true;
        flameThrower.GetComponent<ParticleSystem>().Stop();
        flameThrower.SetActive(false);
        audioSource.Stop();
    }

    private IEnumerator ResetFireDelay()
    {
        yield return new WaitForSeconds(GameMaster.instance.projectileDelay);
        canFire = true;
    }

    private IEnumerator StartAudio()
    {
        audioSource.Play();
        yield return new WaitForSeconds(0.1f);
        firingCoroutine = null;
    }
}
