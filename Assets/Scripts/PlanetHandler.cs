using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;

public class PlanetHandler : MonoBehaviour
{


    public int defaultHealth = 1000;
    public int planetHealth = 1000;

    private ParticleSystem planetaryShieldParticleSystem;
    public CircleCollider2D planetaryCollider;
    private Canvas healthCanvas;

    public bool shieldsUp = true;
    public bool isDead;

    public int planetHits;
    public float reductionPercentage = 0.15f;

    public Material planetMaterial;

    public UnityEngine.Rendering.Universal.Light2D planetLight;

    public Image planetHealthbar;

    private Animator animator;



    private void Start()
    {
        planetaryShieldParticleSystem = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        healthCanvas = GetComponentInChildren<Canvas>();
        healthCanvas.worldCamera = Camera.main;
        planetaryCollider = GetComponent<CircleCollider2D>();
        planetLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {


        Debug.Log(collision.gameObject.name);


        if (!collision.gameObject.name.Contains("EnemyProjectile"))
        {

            if (planetHealth > 0)
            {
                DecrementPlanetHealth(5);

                if (!planetaryShieldParticleSystem)
                {

                    StartCoroutine(flashPlanet());

                }

            }
            else
            {

                animator.SetTrigger("planetsplode");

                Debug.Log("planetsplode");

                if (!isDead)
                {
                    GameMaster.instance.IncrementScore(1000);
                    isDead = true;

                    Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);

                }

            }
        }
    }

    public IEnumerator flashPlanet()
    {
        planetLight.intensity = 1f;
        yield return new WaitForSeconds(0.01f);
        planetLight.intensity = 0f;
    }


    public void DecrementPlanetHealth(int amount)
    {
        // Increment the Player score by the specified amount
        planetHealth -= (amount);
        planetHits += (amount);

        // Update the score text to reflect the new Player score

        float fillAmount = planetHealth / 1000f; // Calculate the fill amount as a ratio
        planetHealthbar.fillAmount = fillAmount; // Assign the fill amount to the Filled Sprite


        if (planetHealth < 750)
        {
            planetHealthbar.color = Color.yellow;
        }

        if (planetHealth < 500)
        {
            planetHealthbar.color = new Color(1f, 0.65f, 0f);

            if (planetaryShieldParticleSystem)
            {
                // Get the particle system's renderer component
                ParticleSystemRenderer renderer = planetaryShieldParticleSystem.GetComponent<ParticleSystemRenderer>();

                // Get the material assigned to the renderer
                Material material = renderer.material;

                // Set the new color for the material
                material.color = Color.red;
            }
        }

        if (planetHealth < 250)
        {
            if (shieldsUp)
            {
                Destroy(planetaryShieldParticleSystem);

                // Calculate the reduced radius
                float reducedRadius = planetaryCollider.radius - (planetaryCollider.radius * reductionPercentage);

                // Set the reduced radius for the collider
                planetaryCollider.radius = reducedRadius;
                shieldsUp = false;
            }


            planetHealthbar.color = Color.red;
        }

    }














}
