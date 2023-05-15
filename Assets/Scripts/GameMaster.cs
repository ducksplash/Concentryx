using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    public CanvasGroup[] PillCanvases;
    public int pillTime;
    public TextMeshProUGUI pillTimeText;

    public bool pillActive;

    public float projectileDelay = 0.2f;

    public int scoreModifier = 1;


    public int health = 100;

    public Slider healthbar;


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
        // Increment the player score by the specified amount
        health -= (amount);

        // Update the score text to reflect the new player score
        healthbar.value = health;

        if (healthbar.value < 0)
        {
            health = 0;
            healthbar.value = health;
        }

        if (healthbar.value < 70)
        {
            healthbar.GetComponentInChildren<Image>().color = Color.yellow;
        }

        if (healthbar.value < 50)
        {
            healthbar.GetComponentInChildren<Image>().color = new Color(1f, 0.65f, 0f);
        }

        if (healthbar.value < 25)
        {
            healthbar.GetComponentInChildren<Image>().color = Color.red;
        }

        if (!isFlashing) // only start the flash effect if not already flashing
        {
            StartCoroutine(FlashScore());
        }

    }


    public void CollectPill(string pilltype)
    {

        StartCoroutine(DisplayPillText(pilltype));

        foreach (CanvasGroup canvasGroup in PillCanvases)
        {
            if (canvasGroup.name == pilltype)
            {
                if (pilltype == "X")
                {
                    projectileDelay = projectileDelay / 4;
                    Debug.Log("projectileDelay: " + projectileDelay);
                }

                if (pilltype == "S")
                {
                    scoreModifier = 2;
                    Debug.Log("projectileDelay: " + projectileDelay);
                }


                pillActive = true;
                canvasGroup.alpha = 1;
                pillTime = 15;
                pillTimeText.GetComponent<CanvasGroup>().alpha = 1;
                StartCoroutine(PillCountDown(canvasGroup, pilltype));
            }
        }

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



    private IEnumerator PillCountDown(CanvasGroup pillCanvas, string pilltype)
    {
        while (pillTime > 0)
        {
            pillTime--;
            pillTimeText.text = pillTime.ToString();
            yield return new WaitForSeconds(1f);
        }

        pillActive = false;

        pillCanvas.alpha = 0f;
        pillTimeText.GetComponent<CanvasGroup>().alpha = 0;

        if (pilltype == "X")
        {
            projectileDelay = 0.1f;
            Debug.Log("projectileDelay: " + projectileDelay);
        }

        if (pilltype == "S")
        {
            scoreModifier = 1;
            Debug.Log("projectileDelay: " + scoreModifier);
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







    private IEnumerator DisplayFloatingText(int val)
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
            floatingTextObj.transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            yield return null;
        }

        // Wait for the duration of the text
        yield return new WaitForSeconds(lifeDuration);

        // Destroy the floating text
        Destroy(floatingTextObj);
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
            floatingTextObj.transform.position -= Vector3.up * riseSpeed * Time.deltaTime;
            yield return null;
        }

        // Wait for the duration of the text
        yield return new WaitForSeconds(lifeDuration);

        // Destroy the floating text
        Destroy(floatingTextObj);
    }







}
