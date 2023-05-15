using UnityEngine;

public class BulletTime : MonoBehaviour
{
    public float countdownTime = 3f;
    private Rigidbody2D rb;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("DestroyGameObject", countdownTime);
    }

    void DestroyGameObject()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        animator.SetTrigger("splode");
        DestroyGameObject();
    }

}
