using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public int playerScore = 0;
    public TextMeshProUGUI scoreText;

    public int countdownTime = 500;
    public TextMeshProUGUI timerText;

    private int currentTime;
    public Color flashColor = Color.red; // The color to flash the brick
    private bool isFlashing;

    [SerializeField] private GameObject textPrefab;
    [SerializeField] private float riseSpeed = 2f;
    [SerializeField] private float lifeDuration = 2f;

    public Material textMaterial;

    public int pillTime;

    public float projectileDelay = 0.1f;
    public float projectileDelayDefault = 0.1f;
    public float projectileDelayBoosted = 0.05f;

    public bool invulnerable = false;
    public int scoreModifier = 1;

    public ScrollRect pillScroll;
    public RectTransform pillContent;
    public GameObject[] pillReadoutPrefabs;


    // weapons
    public string defaultWeapon = "Projectiles";
    public string currentWeapon = "Projectiles";



    public CanvasGroup healthCanvas;
    public int health = 100;
    public int maxHealth = 100;

    public CircleCollider2D shipCollider;
    public float defaultCollider = 0.54f;
    public float shieldCollider = 0.84f;

    public GameObject lightningObject;

    public bool usedLightning;


    int startpilltime = 15;

    public Slider healthbar;



    public ParticleSystem shieldParticleSystem;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Initialize the score text to display the current player score
        scoreText.text = playerScore.ToString();


        timerText.text = currentTime.ToString();

        StartCoroutine(Countdown());

        healthbar.value = health;


        lightningObject.SetActive(false);


    }


    //////  let's try something

    public void InstantiateLevel()
    {
        // instantiate level parts here

        // rings

        // planets

        // enemies

    }


    public void SpawnRings()
    {
        // spawn player here

    }









    public void IncrementScore(int amount)
    {
        // Increment the player score by the specified amount
        playerScore += (amount * scoreModifier);

        // Update the score text to reflect the new player score
        scoreText.text = playerScore.ToString();

        if (!isFlashing) // only start the flash effect if not already flashing
        {
            StartCoroutine(FlashScore());
            StartCoroutine(DisplayFloatingText(amount * scoreModifier));
        }

    }


    public void DecrementHealth(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);
        healthbar.value = health;

        Color healthColor = Color.green;
        float healthValue = healthbar.value;

        switch (healthValue)
        {
            case var _ when healthValue < 70:
                healthColor = Color.yellow;
                break;
            case var _ when healthValue < 50:
                healthColor = new Color(1f, 0.65f, 0f);
                break;
            case var _ when healthValue < 25:
                healthColor = Color.red;
                break;
        }

        healthbar.GetComponentInChildren<Image>().color = healthColor;

        if (!isFlashing)
        {
            StartCoroutine(FlashScore());
            StartCoroutine(FlashHealthBar());
        }
    }


    public void IncrementHealth(int amount)
    {

        if ((health + amount) < maxHealth)
        {
            health += (amount);
        }
        else
        {
            health = maxHealth;
        }


        // Update the score text to reflect the new player score
        healthbar.value = health;

        if (!isFlashing) // only start the flash effect if not already flashing
        {
            StartCoroutine(FlashScore());
            StartCoroutine(FlashHealthBar());
        }

    }

    public void CollectPill(string pilltype)
    {
        StartCoroutine(DisplayPillText(pilltype));
        Debug.Log("Pill collected: " + pilltype);


        if (pilltype == "X")
        {
            projectileDelay = projectileDelayBoosted;
            PillAction(pilltype);
        }

        if (pilltype == "S")
        {
            scoreModifier = 2;
            PillAction(pilltype);
        }


        if (pilltype == "+")
        {
            Debug.Log("Health Pill");
            IncrementHealth(15);
        }

        if (pilltype == "F")
        {
            pillTime = 5;
            Debug.Log("F Pill start" + pillTime);
            PillAction(pilltype);
            currentWeapon = "Flamethrower";



        }

        if (pilltype == "I")
        {
            pillTime = 20;
            Debug.Log("I Pill start" + pillTime);
            PillAction(pilltype);
            invulnerable = true;

            shieldParticleSystem.Play();
            shipCollider.radius = shieldCollider;



        }

        if (pilltype == "L")
        {
            if (!usedLightning)
            {
                StartCoroutine(StrikeLightning());
            }

        }
    }

    public IEnumerator StrikeLightning()
    {
        lightningObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        usedLightning = true;
    }

    private void PillAction(string pilltype)
    {
        string expectedPill = "pilllabel" + pilltype.ToLower();
        insertPillReadout(expectedPill, pilltype);
    }

    public void insertPillReadout(string expectedPill, string pilltype)
    {
        GameObject pillReadoutClone = Instantiate(pillReadoutPrefabs[FindGameObjectIndex(expectedPill)], pillContent);

        RectTransform pillReadoutCloneRectTransform = pillReadoutClone.GetComponent<RectTransform>();
        pillReadoutCloneRectTransform.anchorMin = new Vector2(0.5f, 1f); // Anchored to the top
        pillReadoutCloneRectTransform.anchorMax = new Vector2(0.5f, 1f); // Anchored to the top
        pillReadoutCloneRectTransform.pivot = new Vector2(0.5f, 1f); // Pivot at the top
        pillReadoutCloneRectTransform.anchoredPosition = Vector2.zero;

        // Get the height of the new pill readout
        float newPillHeight = pillReadoutCloneRectTransform.rect.height;

        // Adjust the positions of existing pill readouts
        for (int i = 0; i < pillContent.childCount - 1; i++)
        {
            RectTransform existingPillRectTransform = pillContent.GetChild(i).GetComponent<RectTransform>();
            Vector2 currentPosition = existingPillRectTransform.anchoredPosition;
            currentPosition.y -= newPillHeight; // Subtract newPillHeight instead of adding
            existingPillRectTransform.anchoredPosition = currentPosition;
        }

        StartCoroutine(PillCountDown(pillReadoutClone, pilltype, pillTime));

        Canvas.ForceUpdateCanvases();
        pillScroll.verticalNormalizedPosition = 1f; // Scroll to the top
    }


    public int FindGameObjectIndex(string name)
    {
        int index = Array.FindIndex(pillReadoutPrefabs, obj => obj.name == name);
        Debug.Log("Index of " + name + " is " + index);
        return index;
    }

    private IEnumerator PillCountDown(GameObject pillReadout, string pilltype, int superPillTime)
    {
        GameObject childObject = pillReadout.transform.GetChild(0).gameObject;
        TextMeshProUGUI pilltimereadout = childObject.GetComponent<TextMeshProUGUI>();


        int pilltime = (superPillTime > 0) ? superPillTime : startpilltime;

        pilltimereadout.text = pilltime.ToString();

        while (pilltime > 0)
        {
            pilltime--;
            yield return new WaitForSeconds(1f);
            pilltimereadout.text = pilltime.ToString();
        }

        if (pilltype == "X")
        {
            projectileDelay = projectileDelayDefault;
        }

        if (pilltype == "S")
        {
            scoreModifier = 1;
        }

        if (pilltype == "F")
        {
            Debug.Log("F Pill");
            currentWeapon = defaultWeapon;
        }

        if (pilltype == "I")
        {
            Debug.Log("i Pill");
            invulnerable = false;
            shieldParticleSystem.Stop();
            shipCollider.radius = defaultCollider;
        }

        Destroy(pillReadout);
    }



    private IEnumerator Countdown()
    {
        while (countdownTime > 0)
        {
            countdownTime--;
            timerText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
        }
    }


    private IEnumerator FlashScore()
    {
        isFlashing = true;

        Color originalColor = scoreText.color;

        scoreText.color = flashColor;

        textMaterial.EnableKeyword("UNDERLAY_ON");


        // Wait for the flash duration
        yield return new WaitForSeconds(0.1f);

        scoreText.color = originalColor;

        // Check if the material has the "_LocalLighting" property before setting it to 0

        textMaterial.DisableKeyword("UNDERLAY_ON");

        isFlashing = false;
    }


    private IEnumerator FlashHealthBar()
    {

        healthCanvas.alpha = 1f;


        // Wait for the flash duration
        yield return new WaitForSeconds(0.1f);


        healthCanvas.alpha = 0f;

    }







    private IEnumerator DisplayFloatingText(int val)
    {

        if (val > 0)
        {

            Vector3 offset = Vector3.up * -1.5f; // Change the Y value as needed
            GameObject floatingTextObj = Instantiate(textPrefab, transform.position + offset, Quaternion.identity);

            TextMeshProUGUI textMesh = floatingTextObj.GetComponentInChildren<TextMeshProUGUI>();

            Vector3 targetPosition = transform.position + Vector3.up * 5.0f;

            textMesh.color = Color.white;
            textMesh.text = val.ToString();

            // Rise above the origin object
            while (floatingTextObj.transform.position.y < targetPosition.y)
            {
                floatingTextObj.transform.position += Vector3.up * riseSpeed * Time.smoothDeltaTime;
                yield return null;
            }

            // Wait for the duration of the text
            yield return new WaitForSeconds(lifeDuration);

            // Destroy the floating text
            Destroy(floatingTextObj);
        }

    }






    private IEnumerator DisplayPillText(string val)
    {

        Vector3 offset = Vector3.up * -1.5f; // Change the Y value as needed
        GameObject floatingTextObj = Instantiate(textPrefab, transform.position + offset, Quaternion.identity);

        TextMeshProUGUI textMesh = floatingTextObj.GetComponentInChildren<TextMeshProUGUI>();

        Vector3 targetPosition = transform.position - Vector3.up * 15.0f;

        textMesh.color = Color.red;
        textMesh.text = val;

        // Rise above the origin object
        while (floatingTextObj.transform.position.y > targetPosition.y)
        {
            floatingTextObj.transform.position -= Vector3.up * riseSpeed * Time.smoothDeltaTime;
            yield return null;
        }

        // Wait for the duration of the text
        yield return new WaitForSeconds(lifeDuration);

        // Destroy the floating text
        Destroy(floatingTextObj);
    }







}
