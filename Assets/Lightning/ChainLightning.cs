﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChainLightning : MonoBehaviour
{
    public static ChainLightning instance;

    [Header("Prefabs")]
    public GameObject lineRendererPrefab;
    public GameObject lightRendererPrefab;

    [Header("Config")]
    public int chainLength;
    public int lightnings;

    public string targetTag;

    private float nextRefresh;
    private float segmentLength = 0.2f;

    private List<LightningBolt> lightningBolts;
    private List<Transform> targets;

    private bool isFiring;
    private bool isCleaning;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // InitialiseLightning();
    }

    public void InitialiseLightning()
    {
        lightningBolts = new List<LightningBolt>();
        targets = new List<Transform>();

        for (int i = 0; i < chainLength; i++)
        {
            LightningBolt tmpLightningBolt = new LightningBolt(segmentLength, i);
            tmpLightningBolt.Init(lightnings, lineRendererPrefab, lightRendererPrefab);
            lightningBolts.Add(tmpLightningBolt);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject enemy in enemies)
        {
            targets.Add(enemy.transform);
        }

        // Ensure chainLength matches targets count
        chainLength = Mathf.Min(chainLength, targets.Count);
        BuildChain();
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
        if (isFiring)
            return;

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
        isFiring = false; // Reset the firing flag
    }

    public void ResetChain()
    {
        foreach (var lightningBolt in lightningBolts)
        {
            lightningBolt.Deactivate();
        }

        targets.Clear();
        BuildChain();

        isFiring = false;
        isCleaning = false;
    }

    public void FireChain()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(FireChainCoroutine());
        }
    }

    private IEnumerator FireChainCoroutine()
    {
        // Start the lightning firing sequence
        // ...

        yield return new WaitForSeconds(5f);

        isFiring = false;
    }
}
