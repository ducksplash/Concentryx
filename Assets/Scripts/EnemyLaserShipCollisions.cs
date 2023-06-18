using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class EnemyLaserShipCollisions : MonoBehaviour
{
    public int enemyHealth = 100;
    public int enemyHits = 0;
    public GameObject jet;
    public GameObject laser;
    public bool isDead;
    public Slider enemyHealthbar;
    public EnemyLaserShipMovement movementScript;
    private Animator animator;

    private void Start()
    {

        animator = GetComponent<Animator>();
        jet.SetActive(true);
        laser.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.name.Contains("Enemy") && !collision.gameObject.name.Contains("Boss"))
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
