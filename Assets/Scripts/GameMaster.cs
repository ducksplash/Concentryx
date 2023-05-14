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



}
