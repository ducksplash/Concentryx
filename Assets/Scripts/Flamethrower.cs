using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private ParticleSystem flameParticleSystem;
    private List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        flameParticleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisions = flameParticleSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisions; i++)
        {
            ParticleCollisionEvent collisionEvent = collisionEvents[i];

            // Access the collider of the collided object
            Collider2D collider = collisionEvent.colliderComponent as Collider2D;

            // Do something with the collider (e.g., check its tag, invoke a method)
            if (collider.name.Contains("Segment"))
            {
                int scoreadd = 0;
                if (!collider.GetComponent<Segment>().isDead)
                {
                    scoreadd = collider.GetComponent<Segment>().health;
                    StartCoroutine(AddScore(scoreadd));
                    scoreadd = collider.GetComponent<Segment>().health = 0;
                    collider.GetComponent<Segment>().isDead = true;
                }

                Destroy(collider.gameObject);
            }

            if (collider.name.Contains("Projectile"))
            {
                Destroy(collider.gameObject);

            }


            if (collider.name.Contains("Enemy"))
            {
                if (collider.GetComponent<EnemyShip>())
                {
                    int scoreadd = 0;
                    if (!collider.GetComponent<EnemyShip>().isDead)
                    {
                        scoreadd = collider.GetComponent<EnemyShip>().enemyHealth;
                        StartCoroutine(AddScore(scoreadd));
                        scoreadd = collider.GetComponent<EnemyShip>().enemyHealth = 0;
                        collider.GetComponent<EnemyShip>().isDead = true;


                        Destroy(collider.gameObject);
                    }
                }
            }
        }
    }

    private IEnumerator AddScore(int scoreadd)
    {
        GameMaster.instance.IncrementScore(scoreadd);
        yield return new WaitForSeconds(0.01f);
    }
}
