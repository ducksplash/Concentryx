using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class ChainLightning : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject lineRendererPrefab;
    public GameObject lightRendererPrefab;

    [Header("Config")]
    public int chainLength;
    public int lightnings;

    private bool isCleaning;

    public string targetTag = "Enemy";

    private float nextRefresh;
    private float segmentLength = 0.2f;

    private List<LightningBolt> lightningBolts;
    private List<Transform> targets;

    private void Awake()
    {
        lightningBolts = new List<LightningBolt>();
        targets = new List<Transform>();

        for (int i = 0; i < chainLength; i++)
        {
            LightningBolt tmpLightningBolt = new LightningBolt(segmentLength, i);
            tmpLightningBolt.Init(lightnings, lineRendererPrefab, lightRendererPrefab);
            lightningBolts.Add(tmpLightningBolt);
        }

        BuildChain();
    }

    public void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject enemy in enemies)
        {
            targets.Add(enemy.transform);
        }

        // Ensure chainLength matches targets count
        chainLength = Mathf.Min(chainLength, targets.Count);
    }

    public void BuildChain()
    {
        targets.Clear();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject enemy in enemies)
        {
            targets.Add(enemy.transform);
        }

        // Ensure chainLength matches targets count
        chainLength = Mathf.Min(chainLength, targets.Count);

        for (int i = 0; i < chainLength; i++)
        {
            lightningBolts[i].Activate();
        }

        // Deactivate any remaining LightningBolt instances if target count is reduced
        for (int i = chainLength; i < lightningBolts.Count; i++)
        {
            lightningBolts[i].Deactivate();
        }
    }


    private void Update()
    {
        if (Time.time > nextRefresh)
        {
            // Create a copy of the targets list
            List<Transform> targetsCopy = new List<Transform>(targets);

            for (int i = 0; i < chainLength; i++)
            {
                if (i < targetsCopy.Count && targetsCopy[i] && targetsCopy[i].gameObject.activeSelf)
                {
                    Vector2 startpos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

                    lightningBolts[i].DrawLightning(startpos, targetsCopy[i].position);
                }
                else
                {
                    if (lightningBolts[i] != null)
                    {
                        lightningBolts[i].Deactivate(); // Deactivate the LightningBolt if the target is destroyed
                    }
                }
            }

            nextRefresh = Time.time + 0.01f;
        }

        // Start killing enemies after iterating over targets
        if (!isCleaning)
        {
            StartCoroutine(CleanSweep());
        }
    }





    private IEnumerator CleanSweep()
    {
        isCleaning = true; // Set a flag to indicate the cleaning process is active

        yield return new WaitForSeconds(0.5f);

        // Remove destroyed targets from the list
        targets.RemoveAll(target => target == null);

        for (int i = 0; i < targets.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);

            if (targets[i] != null && targets[i].gameObject != null && targets[i].gameObject.GetComponent<EnemyShip>() != null)
            {
                targets[i].gameObject.GetComponent<EnemyShip>().DestroyEnemyShip();
            }
        }

        isCleaning = false; // Reset the flag after the cleaning process is finished
    }




}
