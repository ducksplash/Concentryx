using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class BossCollisions : MonoBehaviour
{
    public int enemyHealth = 100;
    public int enemyHits = 0;
    public bool isDead;
    public Slider enemyHealthbar1;
    public Slider enemyHealthbar2;
    public BossOneMovement movementScript;
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
                // do nothing as enemies are allies, just destroy the projectile
                collision.gameObject.GetComponent<EnemyBulletTime>().DestroyGameObject();
                break;
            case "BossProjectile(Clone)":
                // ignore own weapon!
                break;
            case "SpaceShip":
                // boss ship is 'out of phase' and does no damage to SpaceShip
                break;
            case "Projectile(Clone)":
                // player shot at ship
                if (enemyHealth > 0)
                {
                    DecrementEnemyHealth(1);
                    GameMaster.instance.IncrementScore(10);
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
            enemyHealthbar1.value = 0;
            enemyHealthbar2.value = 0;
            GameMaster.instance.IncrementScore(enemyHits);
            isDead = true;

            GameMaster.instance.ActiveEnemies--;
            movementScript.enabled = false;
            explosionSound.Play();
            // this shouldn't even be allowed, but it's happening, live with it.
            Destroy(gameObject.transform.parent.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void DecrementEnemyHealth(int amount)
    {
        enemyHealth -= amount;
        enemyHits += amount;

        enemyHealthbar1.value = enemyHealth;
        enemyHealthbar2.value = enemyHealth;


        if (enemyHealth < 3)
        {
            enemyHealthbar1.GetComponentInChildren<Image>().color = Color.red;
            enemyHealthbar2.GetComponentInChildren<Image>().color = Color.red;
        }
    }
}
