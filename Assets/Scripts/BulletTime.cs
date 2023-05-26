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
        rb.velocity = Vector2.zero;

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name != "SpaceShip" && !other.gameObject.name.Contains("Projectile") && !other.gameObject.name.Contains("Thumbstick"))
        {
            DestroyGameObject();
        }
    }

}
