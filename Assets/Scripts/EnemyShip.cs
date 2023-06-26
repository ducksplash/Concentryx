using UnityEngine;
using UnityEngine.UI;

public class EnemyShip : MonoBehaviour
{
    public GameObject Player;
    public Transform playerTransform;
    public float rotationSpeed = 20f;
    public float minChangeDirectionInterval = 2f;
    public float maxChangeDirectionInterval = 8f;

    public int enemyHealth = 20;
    public int enemyHits = 0;
    private float directionTimer;
    private bool rotateClockwise;

    public GameObject jet;

    public bool isDead;

    public Slider enemyHealthbar;
    private Animator animator;
    private AudioSource explosionSound;

    private void Start()
    {
        ResetDirectionTimer();
        rotateClockwise = true;
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        jet.SetActive(true);
        explosionSound = GetComponent<AudioSource>();
        playerTransform = Player.GetComponent<Transform>();
    }

    private void Update()
    {
        // Rotate the sprite around the specified object
        transform.RotateAround(playerTransform.position, Vector3.forward, rotationSpeed * Time.smoothDeltaTime);

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
        if (!collision.gameObject.name.Contains("Enemy") && !collision.gameObject.name.Contains("Boss"))
        {
            collision.gameObject.GetComponent<BulletTime>().DestroyGameObject();

            if (enemyHealth > 0)
            {
                DecrementEnemyHealth(1);
                GameMaster.instance.IncrementScore(1);
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
            explosionSound.Play();
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
