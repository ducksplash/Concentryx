using UnityEngine;

public class Ship : MonoBehaviour
{


    private ParticleSystem shieldParticleSystem;
    public CircleCollider2D shipCollider;



    private void Start()
    {
        shieldParticleSystem = GetComponent<ParticleSystem>();
        shipCollider = GetComponent<CircleCollider2D>();


    }






    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!GameMaster.instance.invulnerable)
        {

            if (collision.gameObject.name.Contains("EnemyProjectile"))
            {
                collision.gameObject.GetComponent<EnemyBulletTime>().DestroyGameObject();

                if (GameMaster.instance.health > 0)
                {
                    GameMaster.instance.DecrementHealth(1);
                }
                else
                {
                    //Debug.Log("Game Over!");
                }
            }

        }

    }

}
