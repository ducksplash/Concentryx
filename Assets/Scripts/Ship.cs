using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour
{
    public static Ship instance;


    public SpriteRenderer shipSprite;
    private Rigidbody2D rb;

    private float rotationSpeed = 180f; // Adjust the rotation speed as needed
    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        shipSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

    }



    // void Update()
    // {
    //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     Vector3 direction = mousePos - transform.position;
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    // }

    public void Rotate(float angle)
    {
        float rotationInRadians = Mathf.Deg2Rad * angle;
        float amplifiedRotation = rotationSpeed * rotationInRadians;
        rb.MoveRotation(rb.rotation + amplifiedRotation);
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
                    StartCoroutine(FlashPlayer());
                    GameMaster.instance.DecrementHealth(1);
                }
                else
                {
                    //Debug.Log("Game Over!");
                }
            }

        }


        if (collision.gameObject.name.Contains("Segment"))
        {

            Destroy(collision.gameObject);
        }

    }



    public IEnumerator FlashPlayer()
    {
        shipSprite.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        shipSprite.color = Color.white;
    }

}
