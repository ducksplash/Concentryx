using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BossThreeMovement : MonoBehaviour
{

    public enum Orientation
    {
        Top,
        Bottom,
        Left,
        Right
    }

    private Orientation shipOrientation;
    public float durationInSeconds = 10f;

    public float timeToWait = 2f;

    public Color[] travelColors;
    public Color[] idleColors;


    public GameObject bossShip;


    public SpriteRenderer shipSpriteRenderer;


    public GameObject[] targetPositions;
    Vector3 startPosition;
    Vector3 targetPosition;
    private bool isRunning = false;

    public bool inMotion = false;
    public GameObject laserLeft;
    public GameObject laserRight;

    private ParticleSystem laserLeftParticleSystem;
    private ParticleSystem laserRightParticleSystem;


    public GameObject Player;
    Quaternion initialRotation;

    private float colourTime = 0f;
    private float colourDuration = 0.2f;
    private bool isLerpingForward = true;

    private void Start()
    {

        initialRotation = bossShip.transform.rotation;

        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(BossBoatRoutine());

        // remove the visual targets as these are only for dev.
        for (int i = 0; i < targetPositions.Length; i++)
        {
            targetPositions[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        shipSpriteRenderer = bossShip.GetComponent<SpriteRenderer>();

        laserLeftParticleSystem = laserLeft.GetComponent<ParticleSystem>();
        laserRightParticleSystem = laserRight.GetComponent<ParticleSystem>();

        laserLeftParticleSystem.Stop();
        laserLeftParticleSystem.Clear();
        laserRightParticleSystem.Stop();
        laserRightParticleSystem.Clear();


    }



    private void Update()
    {


        if (isLerpingForward)
        {
            colourTime += Time.deltaTime / colourDuration;
            if (colourTime >= 1f)
            {
                colourTime = 1f;
                isLerpingForward = false;
            }
        }
        else
        {
            colourTime -= Time.deltaTime / colourDuration;
            if (colourTime <= 0f)
            {
                colourTime = 0f;
                isLerpingForward = true;
            }
        }

        if (inMotion)
        {
            Color targetColor = isLerpingForward ? travelColors[1] : travelColors[0];
            shipSpriteRenderer.color = Color.Lerp(travelColors[0], targetColor, colourTime);
        }
        else
        {
            Color targetColor = isLerpingForward ? idleColors[1] : idleColors[0];
            shipSpriteRenderer.color = Color.Lerp(idleColors[0], targetColor, colourTime);
        }


    }


    private IEnumerator BossBoatRoutine()
    {
        while (true)
        {
            if (!isRunning)
            {
                isRunning = true;
                yield return StartCoroutine(ShipFloatAway());

                isRunning = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }


    private IEnumerator ShipFloatAway()
    {


        // visit targets 

        while (true)
        {
            startPosition = bossShip.transform.position;

            targetPosition = targetPositions[UnityEngine.Random.Range(0, targetPositions.Length)].transform.position;

            // Set the object's position to the start position
            bossShip.transform.position = startPosition;

            // Start the coroutine to move the object to the target position
            inMotion = true;
            yield return StartCoroutine(MoveObject(bossShip, startPosition, targetPosition, durationInSeconds));
            inMotion = false;

            // Wait for a short period before starting again
            yield return new WaitForSeconds(timeToWait);
        }

    }



    private IEnumerator MoveObject(GameObject objectToMove, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        // Calculate the elapsed time
        float elapsedTime = 0;


        if (laserLeftParticleSystem != null && laserRightParticleSystem != null)
        {
            laserLeftParticleSystem.Play();
            laserRightParticleSystem.Play();
        }

        // Loop until the timer exceeds the duration
        while (elapsedTime < duration)
        {
            // Calculate the next position using Lerp and increment the timer
            objectToMove.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;

            // Wait until next frame
            yield return null;
        }

        // Set the object's position to the end position
        objectToMove.transform.position = endPosition;

        if (laserLeftParticleSystem != null && laserRightParticleSystem != null)
        {
            laserLeftParticleSystem.Stop();
            laserLeftParticleSystem.Clear();
            laserRightParticleSystem.Stop();
            laserRightParticleSystem.Clear();
        }
    }







}
