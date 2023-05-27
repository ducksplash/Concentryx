using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RDG;

public class Ship : MonoBehaviour
{
    public static Ship instance;


    public SpriteRenderer shipSprite;
    private Rigidbody2D rb;
    private AndroidJavaObject vibrator;

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



    void Update()
    {
        // if (!GameMaster.instance.onMobile)
        // {
        //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Vector3 direction = mousePos - transform.position;
        //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //     transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        // }
    }
    public void MoveShip(Vector2 direction)
    {

        // Calculate the angle in degrees based on the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Add an offset of 90 degrees to adjust for the rotation difference
        angle -= 90f;

        // Rotate the game object to the calculated angle
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

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

                    //
                    Vibration.Vibrate(50, 255);

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
