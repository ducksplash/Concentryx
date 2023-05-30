using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EnemyLaserShip : MonoBehaviour
{

    public enum Orientation
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [SerializeField]
    private Orientation shipOrientation;

    public GameObject Player;
    public int enemyHealth = 20;
    public int enemyHits = 0;
    public bool isDead;

    public float durationInSeconds = 60f;


    public Slider enemyHealthbar;

    public GameObject jet;
    public GameObject laser;

    public GameObject emenyLaserShip;

    public GameObject[] targetPositions;

    private Animator animator;

    Vector3 startPosition;
    Vector3 targetPosition;

    private void Start()
    {

        Debug.Log("EnemyLaserShip " + shipOrientation);
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        jet.SetActive(true);
        laser.SetActive(true);
        Invoke("TakeSides", 0.5f);

    }
    public void TakeSides()
    {
        switch (shipOrientation)
        {
            case Orientation.Top:
                emenyLaserShip.transform.rotation = Quaternion.Euler(emenyLaserShip.transform.rotation.eulerAngles.x + 180f, emenyLaserShip.transform.rotation.eulerAngles.y, emenyLaserShip.transform.rotation.eulerAngles.z);
                emenyLaserShip.transform.localPosition = targetPositions[0].transform.localPosition;
                startPosition = emenyLaserShip.transform.localPosition;
                targetPosition = targetPositions[1].transform.localPosition;
                StartCoroutine(ShipFloatAway(durationInSeconds));
                break;

            case Orientation.Bottom:
                emenyLaserShip.transform.localPosition = targetPositions[2].transform.localPosition;
                startPosition = emenyLaserShip.transform.localPosition;
                targetPosition = targetPositions[3].transform.localPosition;
                StartCoroutine(ShipFloatAway(durationInSeconds));
                break;

            case Orientation.Left:
                emenyLaserShip.transform.localPosition = targetPositions[4].transform.localPosition;
                startPosition = emenyLaserShip.transform.localPosition;
                targetPosition = targetPositions[5].transform.localPosition;
                StartCoroutine(ShipFloatAway(durationInSeconds));
                break;

            case Orientation.Right:
                emenyLaserShip.transform.localPosition = targetPositions[6].transform.localPosition;
                startPosition = emenyLaserShip.transform.localPosition;
                targetPosition = targetPositions[7].transform.localPosition;
                StartCoroutine(ShipFloatAway(durationInSeconds));
                break;
        }
    }

    private IEnumerator ShipFloatAway(float durationInSeconds)
    {
        Vector3 startPosition = emenyLaserShip.transform.localPosition;
        Vector3 targetPosition = this.targetPosition;
        float totalDistance = (targetPosition - startPosition).magnitude;
        int numSteps = Mathf.RoundToInt(durationInSeconds * 300f);

        for (int step = 0; step <= numSteps; step++)
        {
            float t = (float)step / numSteps;

            // Calculate the new position using Lerp
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Update the object's position
            emenyLaserShip.transform.localPosition = newPosition;

            // Wait for the next FixedUpdate
            yield return new WaitForFixedUpdate();
        }

        // Ensure the object reaches the target position exactly
        emenyLaserShip.transform.localPosition = targetPosition;

        // Swap the start and target positions to move the ship back
        Vector3 temp = startPosition;
        startPosition = targetPosition;
        targetPosition = temp;

        // Start moving the ship back
        StartCoroutine(ShipFloatAway(durationInSeconds));
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
