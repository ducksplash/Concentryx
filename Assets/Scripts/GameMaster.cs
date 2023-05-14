using UnityEngine;
using TMPro;
using System.Collections;

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

    }

    public void IncrementScore(int amount)
    {
        // Increment the player score by the specified amount
        playerScore += amount;

        // Update the score text to reflect the new player score
        scoreText.text = playerScore.ToString();

        if (!isFlashing) // only start the flash effect if not already flashing
        {
            StartCoroutine(FlashScore());
            StartCoroutine(DisplayFloatingText(amount));
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

    private IEnumerator FlashScore()
    {
        isFlashing = true;

        Color originalColor = scoreText.color;

        scoreText.color = flashColor;

        // Wait for the flash duration
        yield return new WaitForSeconds(0.1f);

        GameMaster.instance.scoreText.color = originalColor;


        isFlashing = false;
    }

    private IEnumerator DisplayFloatingText(int val)
    {

        Vector3 offset = Vector3.up * -1.5f; // Change the Y value as needed
        GameObject floatingTextObj = Instantiate(textPrefab, transform.position + offset, Quaternion.identity);

        TextMeshProUGUI textMesh = floatingTextObj.GetComponentInChildren<TextMeshProUGUI>();

        Vector3 targetPosition = transform.position + Vector3.up * 5.0f;

        textMesh.text = val.ToString();

        Debug.Log("Starting coroutine");
        Debug.Log("Target position: " + targetPosition);

        // Wait for a short delay to make sure the text is displayed before it starts rising up
        yield return new WaitForSeconds(0.1f);

        // Rise above the origin object
        while (floatingTextObj.transform.position.y < targetPosition.y)
        {
            floatingTextObj.transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            Debug.Log("Position: " + floatingTextObj.transform.position);
            yield return null;
        }

        // Wait for the duration of the text
        yield return new WaitForSeconds(lifeDuration);

        // Destroy the floating text
        Destroy(floatingTextObj);
        Debug.Log("Coroutine finished");
    }







}
