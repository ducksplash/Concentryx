using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EnemyLaserShip : MonoBehaviour
{
    public GameObject Player;
    public int enemyHealth = 20;
    public int enemyHits = 0;
    public bool isDead;

    public float durationInSeconds = 60f;



    public Slider enemyHealthbar;

    public GameObject jet;
    public GameObject laser;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        jet.SetActive(true);
        laser.SetActive(true);
        StartCoroutine(ShipFloatRight(durationInSeconds));
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
            laser.SetActive(false);
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    private IEnumerator ShipFloatRight(float durationInSeconds)
    {
        // Get the starting position
        Vector3 startPosition = transform.position;

        // Calculate the target position on the right side of the screen
        float targetX = Camera.main.orthographicSize * Camera.main.aspect + 3f;
        Vector3 targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);

        float totalDistance = (targetPosition - startPosition).magnitude;
        int numSteps = Mathf.RoundToInt(durationInSeconds * 300f);

        for (int step = 0; step <= numSteps; step++)
        {
            float t = (float)step / numSteps;

            // Calculate the new position using Lerp
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Update the object's position
            transform.position = newPosition;

            // Wait for the next FixedUpdate
            yield return new WaitForFixedUpdate();
        }

        // Ensure the object reaches the target position exactly
        transform.position = targetPosition;
        StartCoroutine(ShipFloatLeft(durationInSeconds));
    }


    private IEnumerator ShipFloatLeft(float durationInSeconds)
    {
        // Get the starting position
        Vector3 startPosition = transform.position;

        // Calculate the target position on the left side of the screen
        float targetX = -Camera.main.orthographicSize * Camera.main.aspect - 3f;
        Vector3 targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);

        float totalDistance = (targetPosition - startPosition).magnitude;
        int numSteps = Mathf.RoundToInt(durationInSeconds * 300f);

        for (int step = 0; step <= numSteps; step++)
        {
            float t = (float)step / numSteps;

            // Calculate the new position using Lerp
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Update the object's position
            transform.position = newPosition;

            // Wait for the next FixedUpdate
            yield return new WaitForFixedUpdate();
        }

        // Ensure the object reaches the target position exactly
        transform.position = targetPosition;
        StartCoroutine(ShipFloatRight(durationInSeconds));
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
