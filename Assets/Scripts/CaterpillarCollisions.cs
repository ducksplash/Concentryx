using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;

public class CaterpillarCollisions : MonoBehaviour
{
    public int enemyHealth = 50;

    public int enemyMinHealth = 1;

    public int enemyMaxHealth = 50;
    public int enemyHits = 0;
    public bool isDead;
    public Image enemyHealthbar;
    private Animator animator;
    private AudioSource explosionSound;


    private void Start()
    {

        animator = GetComponent<Animator>();
        explosionSound = GetComponent<AudioSource>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.name)
        {
            case "EnemyProjectile(Clone)":
                // do nothing as enemies are allies
                break;
            case "BossProjectile(Clone)":
                // ignore boss weapon!
                break;
            case "SpaceShip":
                // boss ship is 'out of phase' and does no damage to SpaceShip
                break;
            case "Projectile(Clone)":
                // player shot at ship
                if (enemyHealth > 0)
                {
                    DecrementEnemyHealth(1);
                }
                else
                {
                    DestroyEnemyShip();
                }
                break;
        }

    }

    public void DestroyEnemyShip()
    {
        animator.SetTrigger("shipsplode");

        if (!isDead)
        {
            enemyHealthbar.fillAmount = 0f;
            GameMaster.instance.IncrementScore(enemyHits);
            isDead = true;

            GameMaster.instance.ActiveEnemies--;

            // play explosion sound
            explosionSound.Play();
            // this shouldn't even be allowed, but it's happening, live with it.
            Destroy(gameObject.transform.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
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

        // Make sure the fill value is within the valid range
        fillValue = Mathf.Clamp01(fillValue);

        // Update the fill amount of the health bar image
        enemyHealthbar.fillAmount = fillValue;
        if (enemyHealth < 400)
        {
            enemyHealthbar.color = Color.yellow;
        }

        if (enemyHealth < 250)
        {
            enemyHealthbar.color = new Color(1f, 0.65f, 0f);
        }

        if (enemyHealth < 100)
        {
            enemyHealthbar.color = Color.red;
        }

    }
}
