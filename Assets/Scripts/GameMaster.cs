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



    [Header("Player Values")]

    public int playerScore = 0;
    public bool invulnerable = false;
    public int scoreModifier = 1;

    public int playerRank;

    public int playerXP;

    public int toNextRank = 1000;

    public Image XPimg;

    public TextMeshProUGUI rankeryText;

    public TextMeshProUGUI rankMenuText;
    public TextMeshProUGUI rankToMenuText;

    public CanvasGroup healthCanvas;

    public bool deviceVibrationEnabled;
    public int health = 250;
    public int maxHealth = 250;

    public int defaultMaxHealth = 250;

    public CircleCollider2D shipCollider;
    public float defaultCollider = 0.54f;
    public float shieldCollider = 0.84f;

    public Slider healthbar;


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

    public GameObject radialControl;

    public int pillTime;

    public float projectileDelay = 0.1f;
    public float projectileDelayDefault = 0.1f;
    public float projectileDelayBoosted = 0.05f;


    public ScrollRect pillScroll;
    public RectTransform pillContent;
    public GameObject[] pillReadoutPrefabs;


    public int rapidFireTime = 25;
    public int scoreMultiplierTimer = 20;
    public int invulnerabiltyTimer = 20;
    public int flameThrowerTimer = 4;



    public float targetAspectRatio = 16f / 9f;



    // weapons
    public string defaultWeapon = "Projectiles";
    public string currentWeapon = "Projectiles";

    public SpriteRenderer BGSprite;
    public Sprite[] BGSpriteList;

    public bool onMobile = true;

    public GameObject lightningObject;

    public bool usedLightning;


    public Sprite GUIDamageSprite0;
    public Sprite GUIDamageSprite1;
    public Sprite GUIDamageSprite2;
    public Sprite GUIDamageSprite3;

    public SpriteRenderer GUIForegroundSprite;


    int startpilltime = 15;




    public GameObject shieldParticleSystem;





    private void Awake()
    {
        instance = this;

    }




    void Start()
    {


        // Load player rank and player XP from PlayerPrefs
        playerRank = PlayerPrefs.GetInt("PlayerRank", 0);
        playerXP = PlayerPrefs.GetInt("PlayerXP", 0);

        // Update UI with loaded values
        rankeryText.text = playerRank.ToString();
        rankMenuText.text = playerRank.ToString();
        rankToMenuText.text = (toNextRank - playerXP).ToString() + "XP";

        if (toNextRank == 0)
        {
            XPimg.fillAmount = 1f; // Completely filled when playerXP is equal to toNextRank
        }
        else
        {
            XPimg.fillAmount = (float)playerXP / toNextRank;
        }


        // Initialize the score text to display the current player score
        scoreText.text = playerScore.ToString();


        timerText.text = currentTime.ToString();

        StartCoroutine(Countdown());

        healthbar.value = health;


        lightningObject.SetActive(false);



        shieldParticleSystem.GetComponent<ParticleSystem>().Stop();
        shieldParticleSystem.SetActive(false);

        if (BGSpriteList.Length > 0)
        {
            // Select a random index from the BGSpriteList
            int randomIndex = UnityEngine.Random.Range(0, BGSpriteList.Length);

            // Assign the randomly selected sprite to the BGSprite renderer
            BGSprite.sprite = BGSpriteList[randomIndex];
        }


        // if (PlayerPrefs.HasKey("handed"))
        // {
        //     string handedness = PlayerPrefs.GetString("handed");


        //     if (handedness == "left")
        //     {
        //         radialControl.transform.localPosition = new Vector3(915, -172, 0f);
        //     }

        //     else
        //     {
        //         radialControl.transform.localPosition = new Vector3(315, -172, 0f);
        //     }

        // }
        // else
        // {
        //     radialControl.transform.localPosition = new Vector3(315, -172, 0f);
        // }


        // if (Application.platform == RuntimePlatform.Android)
        // {
        //     radialControl.SetActive(true);
        //     onMobile = true;
        // }
        // else
        // {
        //     radialControl.SetActive(false);
        //     onMobile = false;
        // }


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
        // spawn level furniture here

    }



    public void IncrementLevel()
    {


    }



    public void IncrementScore(int amount)
    {
        // Increment the player score by the specified amount
        playerScore += (amount * scoreModifier);

        // Update the score text to reflect the new player score
        scoreText.text = playerScore.ToString();

        ranking(amount * scoreModifier);

        if (!isFlashing) // only start the flash effect if not already flashing
        {
            StartCoroutine(FlashScore());
            StartCoroutine(DisplayFloatingText(amount * scoreModifier));
        }

    }


    public void ranking(int amount)
    {
        playerXP += (int)(amount);
        rankToMenuText.text = (toNextRank - playerXP).ToString() + "XP";

        if (playerXP >= toNextRank)
        {
            playerRank++;
            rankeryText.text = playerRank.ToString();
            rankMenuText.text = playerRank.ToString();
            playerXP = 0;
            toNextRank = Mathf.RoundToInt(toNextRank * 1.3f);

            rankToMenuText.text = (0).ToString() + "XP";
            // get previous
            int originalHealth = maxHealth;

            healthbar.maxValue = Mathf.RoundToInt(healthbar.maxValue * 1.2f);
            maxHealth = Mathf.RoundToInt(maxHealth * 1.1f);

            IncrementHealth(maxHealth - originalHealth);
        }

        if (toNextRank == 0)
        {
            XPimg.fillAmount = 1f; // Completely filled when playerXP is equal to toNextRank
        }
        else
        {
            XPimg.fillAmount = (float)playerXP / toNextRank;
        }

        // Store player rank and player XP to PlayerPrefs
        PlayerPrefs.SetInt("PlayerRank", playerRank);
        PlayerPrefs.SetInt("PlayerXP", playerXP);
        PlayerPrefs.Save();

    }
    public void ResetRank()
    {
        // Reset player rank and XP to defaults
        playerRank = 0;
        playerXP = 0;
        toNextRank = 1000;
        maxHealth = defaultMaxHealth;
        healthbar.maxValue = defaultMaxHealth;
        healthbar.value = defaultMaxHealth;
        health = defaultMaxHealth;

        // Update UI with default values
        rankeryText.text = playerRank.ToString();
        rankMenuText.text = playerRank.ToString();
        rankToMenuText.text = (toNextRank - playerXP).ToString() + "XP";

        if (toNextRank == 0)
        {
            XPimg.fillAmount = 1f; // Completely filled when playerXP is equal to toNextRank
        }
        else
        {
            XPimg.fillAmount = (float)playerXP / toNextRank;
        }

        // Store default player rank and XP to PlayerPrefs
        PlayerPrefs.SetInt("PlayerRank", playerRank);
        PlayerPrefs.SetInt("PlayerXP", playerXP);
        PlayerPrefs.Save();
    }

    public void DecrementHealth(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);
        healthbar.value = health;

        Color healthColor = Color.green;

        if (health < 75)
        {
            healthColor = Color.yellow;
            GUIForegroundSprite.sprite = GUIDamageSprite1;
        }

        if (health < 50)
        {
            healthColor = new Color(1f, 0.65f, 0f);
            GUIForegroundSprite.sprite = GUIDamageSprite2;
        }

        if (health < 25)
        {
            healthColor = Color.red;
            GUIForegroundSprite.sprite = GUIDamageSprite3;
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



    public void CollectPill(string pillType)
    {
        StartCoroutine(DisplayPillText(pillType));

        switch (pillType)
        {
            case "X":
                pillTime = rapidFireTime;
                projectileDelay = projectileDelayBoosted;
                break;
            case "S":
                pillTime = scoreMultiplierTimer;
                scoreModifier = 2;
                break;
            case "+":
                IncrementHealth((UnityEngine.Random.Range(10, 50) * playerRank));
                return;
            case "F":
                pillTime = flameThrowerTimer;
                currentWeapon = "Flamethrower";
                break;
            case "I":
                pillTime = invulnerabiltyTimer;
                invulnerable = true;
                shieldParticleSystem.SetActive(true);
                shieldParticleSystem.GetComponent<ParticleSystem>().Play();
                shipCollider.radius = shieldCollider;
                break;
            case "L":
                if (!usedLightning)
                {
                    StartCoroutine(StrikeLightning());
                }
                return;
        }

        PillAction(pillType);
    }

    public IEnumerator StrikeLightning()
    {
        lightningObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        usedLightning = true;
    }



    private void PillAction(string pillType)
    {
        string expectedPill = "pilllabel" + pillType.ToLower();
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

        StartCoroutine(PillCountDown(pillReadoutClone, pillType, pillTime));

        Canvas.ForceUpdateCanvases();
        pillScroll.verticalNormalizedPosition = 1f; // Scroll to the top
    }




    private IEnumerator PillCountDown(GameObject pillReadout, string pillType, int superPillTime)
    {
        GameObject childObject = pillReadout.transform.GetChild(0).gameObject;
        TextMeshProUGUI pillTimeReadout = childObject.GetComponent<TextMeshProUGUI>();

        int pillTime = (superPillTime > 0) ? superPillTime : startpilltime;
        pillTimeReadout.text = pillTime.ToString();

        while (pillTime > 0)
        {
            pillTime--;

            switch (pillType)
            {
                case "X":
                    projectileDelay = projectileDelayBoosted;
                    break;
                case "S":
                    scoreModifier = 2;
                    break;
                case "F":
                    currentWeapon = "Flamethrower";
                    break;
                case "I":
                    invulnerable = true;
                    shieldParticleSystem.SetActive(true);
                    shieldParticleSystem.GetComponent<ParticleSystem>().Play();
                    shipCollider.radius = shieldCollider;
                    break;
            }

            yield return new WaitForSeconds(1f);
            pillTimeReadout.text = pillTime.ToString();
        }

        switch (pillType)
        {
            case "X":
                projectileDelay = projectileDelayDefault;
                break;
            case "S":
                scoreModifier = 1;
                break;
            case "F":
                currentWeapon = defaultWeapon;
                break;
            case "I":
                invulnerable = false;
                shieldParticleSystem.SetActive(false);
                shieldParticleSystem.GetComponent<ParticleSystem>().Stop();
                shipCollider.radius = defaultCollider;
                break;
        }

        Destroy(pillReadout);
    }

    public int FindGameObjectIndex(string name)
    {
        int index = Array.FindIndex(pillReadoutPrefabs, obj => obj.name == name);
        return index;
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


        string pilltext = val;

        switch (val) // set the text and color based on the pill type
        {
            case "X":
                pilltext = "Rapid Fire";
                break;
            case "S":
                pilltext = "2x Score";
                break;
            case "+":
                pilltext = "Health";
                break;
            case "F":
                pilltext = "The Heat Is On";
                break;
            case "I":
                pilltext = "Shields Up";
                break;
            case "L":
                pilltext = "Buzz Off";
                break;
        }


        textMesh.color = Color.green;
        textMesh.text = pilltext;

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
