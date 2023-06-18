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

    private void Start()
    {

        animator = GetComponent<Animator>();

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
            enemyHealthbar1.value = 0;
            enemyHealthbar2.value = 0;
            GameMaster.instance.IncrementScore(enemyHits);
            isDead = true;

            GameMaster.instance.ActiveEnemies--;
            movementScript.enabled = false;
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
