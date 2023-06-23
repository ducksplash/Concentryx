using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EnemyBuzzbug : MonoBehaviour
{
    public GameObject Player;
    public float rotationSpeed = 100f;
    public float minChangeDirectionInterval = 1f;
    public float maxChangeDirectionInterval = 5.5f;

    public int enemyHealth = 20;
    public int enemyHits = 0;
    private float directionTimer;


    public Vector3 attackOriginPosition;
    public bool isAttacking = false;

    public Coroutine AttackCoroutine;

    public GameObject jet;

    public bool isDead;

    public Slider enemyHealthbar;
    private Animator animator;
    private bool rotateClockwise = true; // Initial rotation direction

    private void Start()
    {
        ResetDirectionTimer();
        rotateClockwise = true;
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        jet.SetActive(true);

    }
    private void Update()
    {
        // Rotate the sprite around the specified object only when not attacking
        if (!isAttacking)
        {
            transform.RotateAround(Player.transform.position, GetRotationAxis(), rotationSpeed * Time.smoothDeltaTime);
        }

        // Update the direction timer
        if (!isAttacking)
        {
            directionTimer -= Time.smoothDeltaTime;

            // Check if it's time to change rotation direction
            if (directionTimer <= 0f)
            {
                // Toggle the rotation direction
                rotateClockwise = !rotateClockwise;

                // Reset the timer with a random interval between minChangeDirectionInterval and maxChangeDirectionInterval
                ResetDirectionTimer();

                // Start the attack coroutine
                StartCoroutine(AttackPlayer());
            }
        }
    }

    // Get the rotation axis based on the current rotation direction
    private Vector3 GetRotationAxis()
    {
        return rotateClockwise ? Vector3.back : Vector3.forward;
    }



    // attack function
    // lerp transform towards player and then lerp back to origin
    public IEnumerator AttackPlayer()
    {
        float distanceThreshold = 0.1f; // Adjust this value as desired

        yield return new WaitForSeconds(0.5f);
        isAttacking = true;

        float attackTime = 0.5f;
        float elapsedTime = 0f;
        float attackSpeed = 1f;

        Vector3 attackStartPosition = transform.position;
        Vector3 playerPosition = Player.transform.position;

        while (elapsedTime < attackTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / attackTime;
            transform.position = Vector3.Lerp(attackStartPosition, playerPosition, t * attackSpeed);

            // Check if the object is close enough to the target
            if (Vector3.Distance(transform.position, playerPosition) <= distanceThreshold)
            {
                break; // Exit the loop prematurely
            }

            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < attackTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / attackTime;
            transform.position = Vector3.Lerp(playerPosition, attackStartPosition, t * attackSpeed);

            // Check if the object is close enough to the starting position
            if (Vector3.Distance(transform.position, attackStartPosition) <= distanceThreshold)
            {
                break; // Exit the loop prematurely
            }

            yield return null;
        }

        isAttacking = false;
    }

    private void ResetDirectionTimer()
    {
        directionTimer = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.name.Contains("Enemy") && !collision.gameObject.name.Contains("Boss"))
        {
            if (collision.gameObject.GetComponent<BulletTime>())
            {
                collision.gameObject.GetComponent<BulletTime>().DestroyGameObject();
            }
            if (enemyHealth > 0)
            {
                DecrementEnemyHealth(1);
            }
            else
            {
                DestroyEnemyShip();
            }
        }
    }

    public void DestroyEnemyShip()
    {
        animator.SetTrigger("shipsplode");

        if (!isDead)
        {
            enemyHealthbar.value = 0;
            GameMaster.instance.IncrementScore(enemyHits);
            isDead = true;
            jet.SetActive(false);

            GameMaster.instance.ActiveEnemies--;
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void DecrementEnemyHealth(int amount)
    {
        // Increment the Player score by the specified amount
        enemyHealth -= amount;
        enemyHits += amount;

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
    }
}
