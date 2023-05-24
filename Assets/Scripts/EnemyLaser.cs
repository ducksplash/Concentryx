using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public GameObject player;

    public GameObject laserObject;

    public bool playerDamaged;

    public float damageDelay = 5f;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "SpaceShip")
        {
            if (!playerDamaged)
            {
                playerDamaged = true;
                if (gameObject.active)
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
            collidee.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            collidee.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            collidee.GetComponent<SpriteRenderer>().color = Color.white;
        }

        yield return new WaitForSeconds(damageDelay);
        playerDamaged = false;
    }


}
