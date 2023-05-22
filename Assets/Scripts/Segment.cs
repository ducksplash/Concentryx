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
    private Material segMaterial;

    public GameObject[] pillPrefabs;

    private float flashTimer;
    private bool isFlashing;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        segMaterial = GetComponent<Renderer>().material;
        defaultMaterialColor = segMaterial.color;

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.offset = Vector2.zero;
        collider.radius = spriteRenderer.bounds.extents.magnitude / 2f;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            DamageSegment(other.transform.gameObject);
        }
    }

    public void DamageSegment(GameObject projectile)
    {
        if (health > 0)
        {
            health--;
            hits++;
            StartFlashPill();
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

    private void StartFlashPill()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            flashTimer = 0.01f;
            segMaterial.color = Color.black;
        }
    }

    private void Update()
    {
        if (isFlashing)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f)
            {
                segMaterial.color = defaultMaterialColor;
                isFlashing = false;
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
            Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
        }
    }
}
