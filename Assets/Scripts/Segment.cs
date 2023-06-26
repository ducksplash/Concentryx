using UnityEngine;
using System.Collections;

public class Segment : MonoBehaviour
{
    public int health = 3;
    public int hits = 0;
    public bool isDead;

    public Color defaultMaterialColor;

    public bool isSpecial;
    private SpriteRenderer spriteRenderer;

    public GameObject[] pillPrefabs;

    private bool isFlashing;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            defaultMaterialColor = spriteRenderer.color;
        }

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

    }


    private void OnCollisionEnter2D(Collision2D other)
    {

        DamageSegment(other.transform.gameObject);

    }

    public void DamageSegment(GameObject projectile)
    {
        if (health > 0)
        {
            health--;
            hits++;
            StartFlashSegment();
        }
        else
        {
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


    private void StartFlashSegment()
    {
        if (spriteRenderer == null)
        {
            return;
        }
        else
        {
            if (!isFlashing)
            {
                isFlashing = true;
                spriteRenderer.color = Color.black;
            }
        }
    }

    public void CreatePill()
    {
        // Determine which pill to print.
        // Set pill weights in the Inspector>Pills>PillPrefab
        // The higher the weight, the more likely the pill will be printed.

        float totalWeight = 0f; // Total weight of all pill prefabs

        foreach (GameObject prefab in pillPrefabs)
        {
            float prefabWeight = prefab.GetComponent<PillX>().weight;
            totalWeight += prefabWeight; // Calculate the total weight by summing up individual prefab weights
        }

        float randomValue = Random.Range(0f, totalWeight); // Generate a random value within the total weight range
        float weightSum = 0f;
        GameObject prefabToInstantiate = null; // The selected prefab to instantiate

        for (int i = 0; i < pillPrefabs.Length; i++)
        {
            GameObject prefab = pillPrefabs[i];
            float prefabWeight = prefab.GetComponent<PillX>().weight;
            weightSum += prefabWeight; // Accumulate the weight sum as we iterate through the prefabs

            if (randomValue <= weightSum)
            {
                prefabToInstantiate = prefab; // Select the prefab if the random value is within the weight sum
                break;
            }
        }

        if (prefabToInstantiate != null)
        {
            GameObject pillfab = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
            pillfab.transform.parent = transform.parent.parent; // Instantiate the selected prefab and set its parent
        }
    }

}
