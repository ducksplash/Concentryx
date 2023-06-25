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

    public float rotationSpeed = 0.7f;

    public Material planetMaterial;
    public Image planetHealthbar;

    public GameObject PlanetSphere;


    public Coroutine planetRotationCoroutine;

    private EnemyProjectile enemyProjectileScript;

    public ParticleSystem planetaryShieldParticleSystem;
    public ParticleSystem atmosphereParticleSystem;
    private CircleCollider2D planetaryCollider;
    private Canvas healthCanvas;
    private UnityEngine.Rendering.Universal.Light2D planetLight;
    private Animator animator;

    private Color defaultColor;
    private Color defaultEmissionColor;
    private bool isHeatingUp = false;
    private const float healthFillAmount = 0.001f;

    private AudioSource explosionSound;

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

        defaultColor = planetMaterial.color;
        defaultEmissionColor = planetMaterial.GetColor("_EmissionColor");

        explosionSound = GetComponent<AudioSource>();
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
            {
                StartCoroutine(FlashPlanet());
            }
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

        float ranDirX = (float)UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        float ranDirY = (float)UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;



        while (PlanetSphere != null)
        {

            PlanetSphere.transform.Rotate(((rotationSpeed * 0.4f) * Time.deltaTime) * ranDirY, (rotationSpeed * Time.deltaTime) * ranDirY, 0f);
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
        GameMaster.instance.ActiveEnemies--;
        explosionSound.Play();
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


        if (planetHealth < 150)
        {
            if (atmosphereParticleSystem != null)
            {
                Destroy(atmosphereParticleSystem);
            }
        }
    }



    public void StartHeatingUp()
    {
        if (!isHeatingUp)
        {
            isHeatingUp = true;
            StartCoroutine(HeatUp());
        }
    }
    public IEnumerator HeatUp()
    {

        Color startColor = planetMaterial.color;
        Color targetColor = Color.yellow;
        float duration = 3f;
        float elapsedTime = 0f;

        Color startEmissionColor = planetMaterial.GetColor("_EmissionColor");
        Color targetEmissionColor = Color.yellow;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Color newColor = Color.Lerp(startColor, targetColor, t);
            Color newEmissionColor = Color.Lerp(startEmissionColor, targetEmissionColor, t);

            planetMaterial.color = newColor;
            planetMaterial.SetColor("_EmissionColor", newEmissionColor);

            planetHealthbar.fillAmount -= 0.0015f;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        Destroy(planetaryShieldParticleSystem);

        yield return new WaitForSeconds(0.05f);

        Destroy(atmosphereParticleSystem);
        animator.SetTrigger("planetsplode");
        planetMaterial.color = defaultColor;
        planetMaterial.SetColor("_EmissionColor", defaultEmissionColor);
        StartCoroutine(Destroy());

        isHeatingUp = false; // Reset the flag
    }
}
