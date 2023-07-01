using UnityEngine;
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

    public Coroutine EnemySweepRoutine;
    public string targetTag;

    private float nextRefresh;
    private float segmentLength = 0.2f;

    [SerializeField] private List<LightningBolt> lightningBolts;
    [SerializeField] private List<Transform> targets;

    public bool engaged;
    private bool isKilling;
    public GameObject gridParent;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // InitialiseLightning();
        engaged = false;
    }


    public void InitialiseLightning()
    {
        if (targets != null)
        {
            targets.Clear();
        }
        else
        {
            targets = new List<Transform>();
        }

        if (lightningBolts != null)
        {
            lightningBolts.Clear();
        }
        else
        {
            lightningBolts = new List<LightningBolt>();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                targets.Add(enemy.transform);
            }

            // Ensure chainLength matches targets count
            chainLength = targets.Count;

            for (int i = 0; i < chainLength; i++)
            {
                LightningBolt tmpLightningBolt = new LightningBolt(segmentLength, i);
                tmpLightningBolt.gridParent = gridParent;
                tmpLightningBolt.Init(lightnings, lineRendererPrefab, lightRendererPrefab);
                lightningBolts.Add(tmpLightningBolt);
            }
        }
    }

    private void Update()
    {
        if (!engaged)
            return;

        if (targets == null || targets.Count < 1)
            return;

        if (Time.time > nextRefresh)
        {
            // Create a copy of the targets list
            List<Transform> targetsCopy = new List<Transform>(targets);

            for (int i = 0; i < chainLength; i++)
            {
                if (i < targetsCopy.Count && targetsCopy[i] && targetsCopy[i].gameObject.activeSelf)
                {
                    lightningBolts[i].Activate();
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
        if (!isKilling)
        {
            EnemySweepRoutine = StartCoroutine(EnemySweep());
        }
    }

    private IEnumerator EnemySweep()
    {
        isKilling = true; // Set a flag to indicate the killing process is active

        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < targets.Count; i++)
        {

            if (targets[i] != null && targets[i].gameObject != null)
            {
                //targets[i].gameObject.GetComponent<EnemyShip>().DestroyEnemyShip();


                if (targets[i].name.Contains("EnemyLaserShip"))
                {
                    targets[i].gameObject.GetComponent<EnemyLaserShipCollisions>().DestroyEnemyShip();
                }

                if (targets[i].name.Contains("EnemyShip"))
                {
                    targets[i].gameObject.GetComponent<EnemyShip>().DestroyEnemyShip();
                }

                if (targets[i].name.Contains("EnemySeg"))
                {
                    targets[i].gameObject.GetComponent<CaterpillarCollisions>().DestroyEnemyShip();
                }

                if (targets[i].name.Contains("EnemyKamikaziMothership"))
                {
                    targets[i].gameObject.GetComponent<EnemyKamikaziMothership>().DestroyEnemyShip();
                }

                if (targets[i].name.Contains("EnemybuzzBug"))
                {
                    targets[i].gameObject.GetComponent<EnemyBuzzbug>().DestroyEnemyShip();
                }
            }

        }

        // Remove destroyed targets from the list
        targets.RemoveAll(target => target == null);
        ResetChain();
    }

    public void ResetChain()
    {
        foreach (LightningBolt lightningBolt in lightningBolts)
        {
            lightningBolt.DestroyLightning();
        }
        lightningBolts = new List<LightningBolt>();
        targets = new List<Transform>();
        isKilling = false;
        engaged = false;
    }
}
