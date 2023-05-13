using UnityEngine;

public class BulletTime : MonoBehaviour
{
    public float countdownTime = 5f;

    void Start()
    {
        Invoke("DestroyGameObject", countdownTime);
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DestroyGameObject();
    }
}
