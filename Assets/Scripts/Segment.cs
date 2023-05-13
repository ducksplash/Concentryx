using UnityEngine;

public class Segment : MonoBehaviour
{

    public int health = 5;

    void Start()
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Add a CircleCollider2D component to the segment object.
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        collider.offset = new Vector2(0f, 0f); // Set the collider offset to the center of the segment

        // Set the radius of the collider to half the sprite width.
        collider.radius = spriteRenderer.bounds.extents.magnitude;

        // Set the isTrigger property to true, so that collisions are detected without physical interaction.
        collider.isTrigger = true;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the segment that was hit by the projectile.
        GameObject projectile = other.transform.gameObject;

        if (other.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {

            // Output a message to the console.
            Debug.Log("Projectile hit segment: " + projectile.name);

            if (health > 0)
            {
                health--;
            }
            else
            {
                // Destroy the segment.
                Destroy(gameObject);
            }
        }

    }
}
