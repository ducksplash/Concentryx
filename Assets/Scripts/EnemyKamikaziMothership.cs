using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;

public class EnemyKamikaziMothership : MonoBehaviour
{
    public GameObject Player;

    public int enemyHealth = 100;

    public int enemyMinHealth = 1;

    public int enemyMaxHealth = 100;

    public GameObject dronePrefab;

    public GameObject droneAperture;

    public int enemyHits = 0;

    public bool isDead;

    private Animator animator;

    private AudioSource explosionSound;

    public Slider enemyHealthbar;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(DeployDrones());
        // Calculate the direction from the object to the player in 2D
        Vector2 direction = Player.transform.position - transform.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the object to face the player
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        explosionSound = GetComponent<AudioSource>();
        ChainLightning.instance.InitialiseLightning();
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
                    GameMaster.instance.IncrementScore(5);

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
            enemyHealthbar.value = 0;
            GameMaster.instance.IncrementScore(enemyHits);
            isDead = true;

            GameMaster.instance.ActiveEnemies--;
            explosionSound.Play();
            // this shouldn't even be allowed, but it's happening, live with it.
            Destroy(gameObject.transform.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }


    public IEnumerator DeployDrones()
    {
        while (gameObject != null)
        {

            GameObject enemyShip = Instantiate(dronePrefab, droneAperture.transform.position, Quaternion.identity);
            enemyShip.transform.parent = droneAperture.transform;
            yield return new WaitForSeconds(0.7f);

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
