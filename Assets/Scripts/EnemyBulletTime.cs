using UnityEngine;

public class EnemyBulletTime : MonoBehaviour
{
    public float countdownTime = 3f;
    private Rigidbody2D rb;

    private Animator animator;

    void Start()
    {
        Invoke("DestroyGameObject", countdownTime);
    }

    public void DestroyGameObject()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("splode");
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Enemy" && !other.gameObject.name.Contains("Projectile"))
        {

            DestroyGameObject();

        }
    }

}