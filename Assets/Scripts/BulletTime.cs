using UnityEngine;

public class BulletTime : MonoBehaviour
{
    public float countdownTime = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyGameObject", countdownTime);
    }

    public void DestroyGameObject()
    {
        rb.velocity = Vector2.zero; // Set the velocity of the Rigidbody2D component to zero

        Destroy(gameObject); // Destroy the game object
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
