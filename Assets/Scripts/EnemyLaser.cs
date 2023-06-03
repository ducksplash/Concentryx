using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public GameObject laserObject;

    public bool playerDamaged;

    public float damageDelay = 5f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "SpaceShip")
        {
            if (!playerDamaged)
            {
                playerDamaged = true;
                if (gameObject.activeSelf)
                {
                    StartCoroutine(DamagePlayer(collider));
                }
            }
        }

    }

    public IEnumerator DamagePlayer(Collider2D collidee)
    {
        GameMaster.instance.DecrementHealth(10);

        if (collidee.GetComponent<SpriteRenderer>())
        {
            collidee.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            collidee.GetComponent<SpriteRenderer>().color = Color.green;
            yield return new WaitForSeconds(0.1f);
            collidee.GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(0.1f);
            collidee.GetComponent<SpriteRenderer>().color = Color.white;
        }

        yield return new WaitForSeconds(damageDelay);
        playerDamaged = false;
    }


}
