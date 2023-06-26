using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BulletTime : MonoBehaviour
{
    public float countdownTime = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterDelay(countdownTime));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        DestroyGameObject();
    }

    public void DestroyGameObject()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        Destroy(gameObject);
    }

    // Handle collision events with other 2D objects
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the collided object is not the "SpaceShip" and does not contain "Projectile"
        if (other.gameObject.name != "SpaceShip" && !other.gameObject.name.Contains("Projectile"))
        {
            DestroyGameObject(); // Destroy the game object
        }
    }
}
