using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;
using System;

public class PlanetHandler : MonoBehaviour
{
    public int defaultHealth = 1000;
    public int planetHealth = 1000;
    public bool shieldsUp = true;
    public bool isDead;
    public float reductionPercentage = 0.15f;

    public float rotationSpeed = 1f;

    public Material planetMaterial;
    public Image planetHealthbar;

    public GameObject PlanetSphere;

    public Coroutine planetRotationCoroutine;

    private EnemyProjectile enemyProjectileScript;

    public ParticleSystem planetaryShieldParticleSystem;
    private CircleCollider2D planetaryCollider;
    private Canvas healthCanvas;
    private UnityEngine.Rendering.Universal.Light2D planetLight;
    private Animator animator;

    private const float healthFillAmount = 0.001f; // Pre-calculated fill amount based on 1000 health

    private void Start()
    {
        enemyProjectileScript = GetComponent<EnemyProjectile>();
        animator = GetComponent<Animator>();
        healthCanvas = GetComponentInChildren<Canvas>();
        healthCanvas.worldCamera = Camera.main;
        planetaryCollider = GetComponent<CircleCollider2D>();
        planetLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        planetLight.intensity = 0f;
        enemyProjectileScript.enabled = false;

        planetRotationCoroutine = StartCoroutine(SlowlyRotate());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
            return;

        if (planetHealth > 0)
        {
            DecrementPlanetHealth(5);
            StartCoroutine(AddScore(10));

            if (!planetaryShieldParticleSystem)
                StartCoroutine(FlashPlanet());
        }
        else
        {
            animator.SetTrigger("planetsplode");
            if (!isDead)
            {
                planetaryCollider.enabled = false;
                StartCoroutine(AddScore(1000));
                isDead = true;
                StopCoroutine(planetRotationCoroutine);
                planetRotationCoroutine = null;
                StartCoroutine(Destroy());
            }
        }



        if (collision.gameObject.name.Contains("Projectile"))
        {

            Destroy(collision.gameObject);
        }


        if (collision.gameObject.name.Contains("Segment"))
        {

            Destroy(collision.gameObject);
        }
    }


    private IEnumerator SlowlyRotate()
    {

        while (true)
        {

            PlanetSphere.transform.Rotate((rotationSpeed * 0.3f) * Time.deltaTime, rotationSpeed * Time.deltaTime, 0f);
            yield return null;

        }
    }


    private IEnumerator AddScore(int scoreadd)
    {
        GameMaster.instance.IncrementScore(scoreadd);
        yield return new WaitForSeconds(0.01f);
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(PlanetSphere);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);

    }

    private IEnumerator FlashPlanet()
    {
        planetLight.intensity = 1f;
        yield return new WaitForSeconds(0.01f);
        planetLight.intensity = 0f;
    }

    public void DecrementPlanetHealth(int amount)
    {
        planetHealth -= amount;
        float fillAmount = planetHealth * healthFillAmount;
        planetHealthbar.fillAmount = fillAmount;

        if (planetHealth < 750)
            planetHealthbar.color = Color.yellow;

        if (planetHealth < 500)
        {
            planetHealthbar.color = new Color(1f, 0.65f, 0f);
            if (planetaryShieldParticleSystem)
            {
                ParticleSystemRenderer renderer = planetaryShieldParticleSystem.GetComponent<ParticleSystemRenderer>();
                Material material = renderer.material;
                material.color = Color.red;
            }
        }

        if (planetHealth < 250)
        {
            if (shieldsUp)
            {
                Destroy(planetaryShieldParticleSystem);

                enemyProjectileScript.enabled = true;
                float reducedRadius = planetaryCollider.radius - (planetaryCollider.radius * reductionPercentage);
                planetaryCollider.radius = reducedRadius;
                shieldsUp = false;
            }
            planetHealthbar.color = Color.red;
        }
    }
}
