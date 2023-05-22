using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class PillX : MonoBehaviour
{

    public int health = 6;

    public int hits = 0;
    public float flashDuration = 0.1f; // How long to flash the brick after being hit

    public Color flashColor = Color.red; // The color to flash the brick

    public Color lightColor = Color.white; // The color to flash the brick
    private bool isFlashing;

    public bool isDead;
    private SpriteRenderer spriteRenderer;

    private string pilltype;

    public float weight = 0;

    public GameObject[] pillPrefabs;


    void Start()
    {

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        string modifiedString = gameObject.name.Replace("(Clone)", "");


        pilltype = modifiedString.Substring(modifiedString.Length - 1)[0].ToString();


        // Add a CircleCollider2D component to the pill object.
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Set the collider offset to the center of the pill
        collider.offset = new Vector2(0f, 0f);

        // Set the radius of the collider 
        collider.radius = spriteRenderer.bounds.extents.magnitude / 2f;

        // Set the isTrigger property to true, so that collisions are detected without physical interaction.
        //collider.isTrigger = true;
    }




    private void OnCollisionEnter2D(Collision2D other)
    {
        // Get the pill that was hit by the projectile.
        GameObject projectile = other.transform.gameObject;

        if (other.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {

            // Output a message to the console.

            if (health > 0)
            {
                health--;
                hits++;
                if (!isFlashing) // only start the flash effect if not already flashing
                {
                    StartCoroutine(FlashPill());
                }

            }
            else
            {

                if (!isDead)
                {


                    GameMaster.instance.CollectPill(pilltype);
                    StartCoroutine(AddScore(hits * 2));
                    isDead = true;
                }

                Destroy(gameObject);
            }
        }

    }

    private IEnumerator AddScore(int scoreadd)
    {
        GameMaster.instance.IncrementScore(scoreadd);
        yield return new WaitForSeconds(0.01f);
    }


    private IEnumerator FlashPill()
    {
        isFlashing = true;

        gameObject.AddComponent<Light2D>();

        Light2D flashingLight = gameObject.GetComponent<Light2D>();

        // Save the original color of the brick
        Color originalColor = spriteRenderer.color;

        // Change the color of the brick to the flash color
        spriteRenderer.color = flashColor;

        flashingLight.enabled = true;
        flashingLight.color = lightColor;

        // Set the range of the light to be just over the size of the object
        float radius = Mathf.Max(transform.localScale.x, transform.localScale.y) / 3f;
        flashingLight.pointLightOuterRadius = radius;

        // Set the intensity of the light
        flashingLight.intensity = 40f;

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Change the color of the brick back to the original color
        spriteRenderer.color = originalColor;

        // Destroy the light component
        Destroy(flashingLight);

        isFlashing = false;
    }




    public void SpawnPill()
    {

    }





}
