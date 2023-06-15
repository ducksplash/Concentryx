using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public GameObject Concentryx;

    [Header("Player Values")]

    public int playerScore = 0;
    public int playerScoreThisLevel = 0;
    public int playerHighScore = 0;
    public bool invulnerable = false;
    public int scoreModifier = 1;

    public int playerRank;

    public int playerXP;

    public int toNextRank;

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
    public Slider rankSlider;


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextRanks;
    public TextMeshProUGUI highScoreText;

    public int CurrentLevel = 1;


    public bool LevelEngaged = false;

    public CanvasGroup levelEndCanvas;

    public TextMeshProUGUI levelEndText;

    public TextMeshProUGUI levelEndScoreText;
    public TextMeshProUGUI levelEndScoreTitleText;
    public TextMeshProUGUI levelEndScoreThisLevelText;
    public TextMeshProUGUI levelEndHighScoreText;

    public Button levelEndNextLevelButton;
    public Button levelEndRetryButton;

    public CanvasGroup levelEndButtonsParent;

    public TextMeshProUGUI timerText;

    private int currentTime;
    public Color flashColor = Color.red; // The color to flash the brick
    private bool isFlashing;

    [SerializeField] private GameObject textPrefab;
    [SerializeField] private float riseSpeed = 2f;
    [SerializeField] private float lifeDuration = 2f;

    public Material textMaterial;

    public int healthLootValue = 0;

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


    public Sprite GUIDamageSprite0;
    public Sprite GUIDamageSprite1;
    public Sprite GUIDamageSprite2;
    public Sprite GUIDamageSprite3;

    public SpriteRenderer GUIForegroundSprite;


    int startpilltime = 15;

    public int ActiveEnemies = 0;


    public GameObject shieldParticleSystem;

    public VolumeProfile PPVolumeProfile;
    private ColorAdjustments colorAdjustments;

    private bool colorAdjustAvailable = false;

    public GameObject phoneControl;

    public AudioMixer AudioMixer;

    public CanvasGroup cheatMenuCanvas;

    public bool playerReady = true;


    private void Awake()
    {
        instance = this;

    }




    void Start()
    {

        playerHighScore = PlayerPrefs.GetInt("PlayerHighScore", 0);

        if (PPVolumeProfile.TryGet(out colorAdjustments))
        {
            colorAdjustAvailable = true;
        }
        else
        {
            colorAdjustAvailable = false;
        }


        // Load player rank and player XP from PlayerPrefs
        playerRank = PlayerPrefs.GetInt("PlayerRank", 0);
        playerXP = PlayerPrefs.GetInt("PlayerXP", 0);
        toNextRank = PlayerPrefs.GetInt("ToNextRank", 1000);

        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        // Update UI with loaded values
        rankeryText.text = playerRank.ToString();
        rankMenuText.text = playerRank.ToString();

        int minNextRank = (toNextRank - playerXP) < 0 ? 0 : (toNextRank - playerXP);

        rankToMenuText.text = minNextRank.ToString() + "XP";

        rankSlider.maxValue = toNextRank;
        rankSlider.value = playerXP;

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

        scoreTextRanks.text = playerScore.ToString();

        highScoreText.text = playerHighScore.ToString();

        timerText.text = currentTime.ToString();

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




        if (Application.platform == RuntimePlatform.Android)
        {
            phoneControl.SetActive(true);
            onMobile = true;
        }
        else
        {
            phoneControl.SetActive(false);
            onMobile = false;
        }
        cheatMenuCanvas.alpha = 0;
        cheatMenuCanvas.interactable = false;
        cheatMenuCanvas.blocksRaycasts = false;
        levelEndNextLevelButton.interactable = false;
        levelEndRetryButton.interactable = false;
        ResetColourAdjustments();
    }



    //////  let's try something

    public void InstantiateLevel()
    {

        ActiveEnemies = 0;
        // instantiate the next expected level
        // 
        Concentryx.GetComponent<Concentryx>().BuildLevel(CurrentLevel);

        // set to 'playing'
        LevelEngaged = true;

    }


    public void Update()
    {

        if (LevelEngaged)
        {
            if (ActiveEnemies == 0)
            {
                LevelEngaged = false;
                StartCoroutine(EndLevel(0));
            }
        }


        if (playerReady)
        {
            levelEndButtonsParent.alpha = 1f;
            levelEndButtonsParent.interactable = true;
            levelEndButtonsParent.blocksRaycasts = true;
        }
        else
        {
            levelEndButtonsParent.alpha = 0f;
            levelEndButtonsParent.interactable = false;
            levelEndButtonsParent.blocksRaycasts = false;
        }


    }


    public void ResetColourAdjustments()
    {
        if (colorAdjustAvailable)
        {
            colorAdjustments.saturation.value = 0f;
            colorAdjustments.postExposure.value = 0f;

        }
    }

    // should spin this off into dedicated level manager

    public IEnumerator EndLevel(int reason = 0)
    {
        Time.timeScale = 0f;


        AudioMixer.SetFloat("SFX", -40f);
        AudioMixer.SetFloat("BGM", -40f);

        if (colorAdjustAvailable)
        {
            colorAdjustments.saturation.value = -100f;
            colorAdjustments.postExposure.value = 10f;


            while (colorAdjustments.postExposure.value > 0)
            {
                colorAdjustments.postExposure.value -= 0.1f;
                yield return new WaitForSecondsRealtime(0.05f);
            }

        }


        levelEndCanvas.alpha = 1f;
        levelEndCanvas.interactable = true;
        levelEndCanvas.blocksRaycasts = true;

        // level complete by destroying all enemies
        if (reason == 0)
        {
            ResetColourAdjustments();
            levelEndNextLevelButton.interactable = true;
            levelEndRetryButton.interactable = false;
            levelEndText.text = "Level " + CurrentLevel.ToString() + " Complete!";
            levelEndScoreTitleText.text = "Level " + CurrentLevel.ToString() + " Score";
            levelEndScoreText.text = playerScore.ToString();
            levelEndScoreThisLevelText.text = playerScoreThisLevel.ToString();
            levelEndHighScoreText.text = playerHighScore.ToString();


            playerScoreThisLevel = 0;

            CurrentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);

        }


        // level ended by running out of time
        if (reason == 1)
        {

            ResetColourAdjustments();
            levelEndNextLevelButton.interactable = false;
            levelEndRetryButton.interactable = true;
            levelEndText.text = "Out Of Time!";
            levelEndScoreTitleText.text = "Level " + CurrentLevel.ToString() + " Score";
            levelEndScoreText.text = playerScore.ToString();
            levelEndScoreThisLevelText.text = playerScoreThisLevel.ToString();
            levelEndHighScoreText.text = playerHighScore.ToString();
            ResetRank();

            playerScoreThisLevel = 0;

        }
        // level ended by death
        if (reason == 2)
        {
            Debug.Log("reason 2");
            ResetColourAdjustments();
            levelEndNextLevelButton.interactable = false;
            levelEndRetryButton.interactable = true;
            levelEndText.text = "You Died!";
            levelEndScoreTitleText.text = "Level " + CurrentLevel.ToString() + " Score";
            levelEndScoreText.text = playerScore.ToString();
            levelEndScoreThisLevelText.text = playerScoreThisLevel.ToString();
            levelEndHighScoreText.text = playerHighScore.ToString();
            ResetRank();

            playerScoreThisLevel = 0;

        }

    }




    public void IncrementScore(int amount)
    {
        // Increment the player score by the specified amount
        playerScore += (amount * scoreModifier);
        playerScoreThisLevel += (amount * scoreModifier);

        // Update the score text to reflect the new player score
        scoreText.text = playerScore.ToString();
        scoreTextRanks.text = playerScore.ToString();


        if (playerScore > playerHighScore)
        {
            playerHighScore = playerScore;
        }

        highScoreText.text = playerHighScore.ToString();


        ranking(amount * scoreModifier);

        PlayerPrefs.SetInt("PlayerHighScore", playerHighScore);
        PlayerPrefs.Save();

        if (!isFlashing) // only start the flash effect if not already flashing
        {
            StartCoroutine(FlashScore());
            StartCoroutine(DisplayFloatingText(amount * scoreModifier));
        }

    }

    public void ranking(int amount)
    {
        playerXP += amount;

        rankToMenuText.text = (toNextRank - playerXP).ToString() + "XP";
        rankSlider.value = playerXP;

        if (playerXP >= toNextRank)
        {
            playerRank++;
            rankeryText.text = playerRank.ToString();
            rankMenuText.text = playerRank.ToString();

            toNextRank = Mathf.CeilToInt(toNextRank * 1.3f);
            playerXP = 0;

            // Update health values
            int originalHealth = maxHealth;
            healthbar.maxValue = Mathf.CeilToInt(healthbar.maxValue * 1.2f);
            maxHealth = Mathf.CeilToInt(maxHealth * 1.1f);
            IncrementHealth(maxHealth - originalHealth);
        }

        if (toNextRank == 0)
        {
            XPimg.fillAmount = 0f; // Completely filled when playerXP is equal to toNextRank
        }
        else
        {
            XPimg.fillAmount = (float)playerXP / toNextRank;
        }


        // Store player rank and player XP to PlayerPrefs
        PlayerPrefs.SetInt("PlayerRank", playerRank);
        PlayerPrefs.SetInt("ToNextRank", toNextRank);
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
        playerHighScore = 0;
        playerScore = 0;
        playerScoreThisLevel = 0;


        // Update UI with default values
        rankeryText.text = playerRank.ToString();
        rankMenuText.text = playerRank.ToString();
        rankToMenuText.text = (toNextRank - playerXP).ToString() + "XP";
        rankSlider.value = playerXP;
        scoreTextRanks.text = playerScore.ToString();
        scoreText.text = playerScore.ToString();
        highScoreText.text = playerHighScore.ToString();


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
        PlayerPrefs.SetInt("ToNextRank", toNextRank);
        PlayerPrefs.SetInt("PlayerScore", playerScore);
        PlayerPrefs.SetInt("PlayerScoreThisLevel", playerScoreThisLevel);
        PlayerPrefs.SetInt("PlayerXP", playerXP);
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
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

        switch (pillType)
        {
            case "X":
                pillTime = rapidFireTime;
                projectileDelay = projectileDelayBoosted;
                StartCoroutine(DisplayPillText(pillType));
                break;
            case "S":
                pillTime = scoreMultiplierTimer;
                scoreModifier = UnityEngine.Random.Range(2, 5);
                StartCoroutine(DisplayPillText(pillType));
                break;
            case "+":
                healthLootValue = (UnityEngine.Random.Range(10, 50));
                IncrementHealth(healthLootValue);
                StartCoroutine(DisplayPillText(pillType));
                return;
            case "F":
                pillTime = flameThrowerTimer;
                currentWeapon = "Flamethrower";
                StartCoroutine(DisplayPillText(pillType));
                break;
            case "I":
                pillTime = invulnerabiltyTimer;
                invulnerable = true;
                shieldParticleSystem.SetActive(true);
                shieldParticleSystem.GetComponent<ParticleSystem>().Play();
                shipCollider.radius = shieldCollider;
                StartCoroutine(DisplayPillText(pillType));
                break;
            case "L":

                StartCoroutine(StrikeLightning());
                StartCoroutine(DisplayPillText(pillType));

                return;
        }

        PillAction(pillType);
    }

    public IEnumerator StrikeLightning()
    {
        lightningObject.SetActive(true);
        ChainLightning.instance.engaged = true;
        yield return new WaitForSeconds(0.5f);
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
                    scoreModifier = UnityEngine.Random.Range(2, 5);
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

        yield return new WaitForSeconds(1f);
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




    public IEnumerator Countdown(int timeleft)
    {
        while (timeleft > 0)
        {
            timeleft--;
            timerText.text = timeleft.ToString();
            yield return new WaitForSeconds(1f);
        }


        StartCoroutine(EndLevel(1));


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
                pilltext = scoreModifier + "x Score";
                break;
            case "+":
                pilltext = "+" + healthLootValue + " Health";
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





    public void CheatMenu()
    {
        if (cheatMenuCanvas.alpha == 0)
        {
            cheatMenuCanvas.alpha = 1;
            cheatMenuCanvas.interactable = true;
            cheatMenuCanvas.blocksRaycasts = true;
        }
        else
        {
            cheatMenuCanvas.alpha = 0;
            cheatMenuCanvas.interactable = false;
            cheatMenuCanvas.blocksRaycasts = false;
        }
    }



}
