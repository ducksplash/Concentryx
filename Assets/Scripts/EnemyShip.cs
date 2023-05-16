using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;


public class EnemyShip : MonoBehaviour
{
    public GameObject Player;
    public float rotationSpeed = 20f;
    public float minChangeDirectionInterval = 2f;
    public float maxChangeDirectionInterval = 8f;

    public int enemyHealth = 20;
    public int enemyHits = 0;
    private float directionTimer;
    private bool rotateClockwise;

    public bool isDead;

    public Slider enemyHealthbar;
    private Animator animator;

    private void Start()
    {
        ResetDirectionTimer();
        rotateClockwise = true;
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        // Rotate the sprite around the specified object
        transform.RotateAround(Player.transform.position, Vector3.forward, rotationSpeed * Time.smoothDeltaTime);

        // Update the timer
        directionTimer -= Time.smoothDeltaTime;

        // Check if it's time to change rotation direction
        if (directionTimer <= 0f)
        {
            // Randomly determine the new rotation direction
            rotateClockwise = Random.value < 0.7f;

            // Reset the timer with a random interval between minChangeDirectionInterval and maxChangeDirectionInterval
            ResetDirectionTimer();
        }

        // Set the rotation direction based on the current setting
        rotationSpeed = rotateClockwise ? Mathf.Abs(rotationSpeed) : -Mathf.Abs(rotationSpeed);
    }

    private void ResetDirectionTimer()
    {
        directionTimer = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!collision.gameObject.name.Contains("EnemyProjectile"))
        {
            collision.gameObject.GetComponent<BulletTime>().DestroyGameObject();

            if (enemyHealth > 0)
            {
                DecrementEnemyHealth(1);
            }
            else
            {

                animator.SetTrigger("shipsplode");

                Debug.Log("shipsplode");

                if (!isDead)
                {
                    GameMaster.instance.IncrementScore(enemyHits);
                    isDead = true;

                    Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);

                }

            }
        }
    }


    public void DecrementEnemyHealth(int amount)
    {
        // Increment the Player score by the specified amount
        enemyHealth -= (amount);
        enemyHits += (amount);

        // Update the score text to reflect the new Player score
        enemyHealthbar.value = enemyHealth;


        if (enemyHealth < 8)
        {
            enemyHealthbar.GetComponentInChildren<Image>().color = Color.yellow;
        }

        if (enemyHealth < 6)
        {
            enemyHealthbar.GetComponentInChildren<Image>().color = new Color(1f, 0.65f, 0f);
        }

        if (enemyHealth < 3)
        {
            enemyHealthbar.GetComponentInChildren<Image>().color = Color.red;
        }

        // if (!isFlashing) // only start the flash effect if not already flashing
        // {
        //     StartCoroutine(FlashScore());
        // }

    }
}
