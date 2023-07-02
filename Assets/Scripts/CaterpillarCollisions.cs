using UnityEngine;
using UnityEngine.UI;

public class CaterpillarCollisions : MonoBehaviour
{
    public int enemyHealth = 50;
    public int enemyMinHealth = 1;
    public int enemyMaxHealth = 50;
    public int enemyHits = 0;

    public Image enemyHealthbar;
    private Animator animator;
    private AudioSource explosionSound;

    private bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        explosionSound = GetComponent<AudioSource>();
        ChainLightning.instance.InitialiseLightning();
        GameMaster.instance.ActiveEnemies++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Projectile(Clone)":
                if (enemyHealth > 0)
                {
                    DecrementEnemyHealth(1);
                    GameMaster.instance.IncrementScore(2);
                }
                else
                {
                    DestroyEnemyShip();
                }
                break;
            case "EnemyProjectile(Clone)":
                // Do nothing as enemies are allies
                break;
            case "BossProjectile(Clone)":
                // Ignore boss weapon
                break;
            case "SpaceShip":
                // Boss ship is 'out of phase' and does no damage to SpaceShip
                break;
        }
    }

    public void DestroyEnemyShip()
    {
        Debug.Log("Destroying caterpillar ship");

        animator.SetTrigger("shipsplode");

        if (!isDead)
        {
            enemyHealthbar.fillAmount = 0f;
            GameMaster.instance.IncrementScore(enemyHits);
            isDead = true;
            GameMaster.instance.ActiveEnemies--;

            Debug.Log("enemies left: " + GameMaster.instance.ActiveEnemies);

            // Play explosion sound
            explosionSound.Play();
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void DecrementEnemyHealth(int amount)
    {
        enemyHealth -= amount;
        enemyHits += amount;

        // Clamp the value within the specified range
        float clampedValue = Mathf.Clamp(enemyHealth, enemyMinHealth, enemyMaxHealth);

        // Map the clamped value to the range 0-1
        float fillValue = (clampedValue - enemyMinHealth) / (enemyMaxHealth - enemyMinHealth);

        // Update the fill amount of the health bar image
        enemyHealthbar.fillAmount = fillValue;

        // Update the health bar color based on enemyHealth value
        if (enemyHealth < 400)
            enemyHealthbar.color = Color.yellow;

        if (enemyHealth < 250)
            enemyHealthbar.color = new Color(1f, 0.65f, 0f);

        if (enemyHealth < 100)
            enemyHealthbar.color = Color.red;
    }
}
