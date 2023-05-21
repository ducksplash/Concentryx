using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Segment : MonoBehaviour
{

    public int health = 3;

    public int hits = 0;
    public bool isDead;



    public bool isSpecial;
    private SpriteRenderer spriteRenderer;

    public GameObject[] pillPrefabs;

    void Start()
    {


        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Add a CircleCollider2D component to the segment object.
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Set the collider offset to the center of the segment
        collider.offset = new Vector2(0f, 0f);

        // Set the radius of the collider 
        collider.radius = spriteRenderer.bounds.extents.magnitude / 2f;

        // Set the isTrigger property to true
        //collider.isTrigger = true;
    }




    private void OnCollisionEnter2D(Collision2D other)
    {
        // Get the segment that was hit by the projectile.
        GameObject projectile = other.transform.gameObject;

        if (other.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            DamageSegment(projectile);

        }

    }



    public void DamageSegment(GameObject projectile)
    {
        if (health > 0)
        {
            health--;
            hits++;
        }
        else
        {
            // Destroy the segment.

            if (!projectile.name.Contains("EnemyProjectile"))
            {


                if (isSpecial)
                {
                    CreatePill();
                }


                if (!isDead)
                {
                    GameMaster.instance.IncrementScore(hits);
                    isDead = true;
                }
            }

            Destroy(gameObject);
        }
    }

    public void CreatePill()
    {
        float totalWeight = 0f;
        foreach (GameObject prefab in pillPrefabs)
        {
            // Assuming each prefab has a weight assigned using a custom script or editor properties
            float prefabWeight = prefab.GetComponent<PillX>().weight;
            totalWeight += prefabWeight;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float weightSum = 0f;
        GameObject prefabToInstantiate = null;

        for (int i = 0; i < pillPrefabs.Length; i++)
        {
            GameObject prefab = pillPrefabs[i];
            float prefabWeight = prefab.GetComponent<PillX>().weight;

            weightSum += prefabWeight;

            if (randomValue <= weightSum)
            {
                prefabToInstantiate = prefab;
                break;
            }
        }

        if (prefabToInstantiate != null)
        {
            Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
        }
    }



}
