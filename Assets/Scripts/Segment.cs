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
                    StartCoroutine(AddScore(hits));
                    isDead = true;
                }
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator AddScore(int scoreadd)
    {
        GameMaster.instance.IncrementScore(scoreadd);
        yield return new WaitForSeconds(0.01f);
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
        float totalWeight = 0f;
        foreach (GameObject prefab in pillPrefabs)
        {
            float prefabWeight = prefab.GetComponent<PillX>().weight;
            totalWeight += prefabWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);
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
            GameObject pillfab = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
            pillfab.transform.parent = transform.parent.parent;

        }
    }
}
