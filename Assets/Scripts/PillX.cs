using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class PillX : MonoBehaviour
{

    public int health = 6;

    public int hits = 0;

    public bool isDead;
    private SpriteRenderer spriteRenderer;

    public Color pillStartColor;
    public Color pillTargetColor;
    private string pilltype;

    public float weight = 0;


    void Start()
    {

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        string modifiedString = gameObject.name.Replace("(Clone)", "");


        pilltype = modifiedString.Substring(modifiedString.Length - 1)[0].ToString();


        // Add a CircleCollider2D component to the pill object.
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        pillStartColor = spriteRenderer.color;
        pillTargetColor = new Color(pillStartColor.r, pillStartColor.g, pillStartColor.b, 0.5f);
        // Set the collider offset to the center of the pill
        collider.offset = new Vector2(0f, 0f);

        // Set the radius of the collider 
        collider.radius = spriteRenderer.bounds.extents.magnitude / 2f;

        StartCoroutine(FlashPill());

    }

    private IEnumerator FlashPill()
    {
        while (true)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.Lerp(pillStartColor, pillTargetColor, Mathf.PingPong(Time.time * 6, 0.8f));
            }

            yield return null; // Yield control back to the coroutine scheduler
        }
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
            }
            else
            {

                if (!isDead)
                {
                    GameMaster.instance.CollectPill(pilltype);
                    GameMaster.instance.IncrementScore(hits * 2);
                    isDead = true;
                }

                DisablePill();
            }
        }

    }


    private void DisablePill()
    {
        // Disable the components of the pill
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }

}
