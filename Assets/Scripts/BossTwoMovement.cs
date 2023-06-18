using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BossTwoMovement : MonoBehaviour
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
    public GameObject leftWing;
    public GameObject rightWing;

    public SpriteRenderer shipSpriteRenderer;
    public SpriteRenderer leftWingSpriteRenderer;
    public SpriteRenderer rightWingSpriteRenderer;

    public GameObject[] targetPositions;
    Vector3 leftStartPosition;
    Vector3 rightStartPosition;
    Vector3 leftTargetPosition;
    Vector3 rightTargetPosition;
    private bool isRunning = false;

    public bool leftInMotion = false;
    public bool rightInMotion = false;

    public GameObject Player;
    Quaternion initialRotation;

    private float colourTime = 0f;
    private float colourDuration = 0.2f;
    private bool isLerpingForward = true;

    public EnemyProjectile enemyProjectileScriptLeft;
    public EnemyProjectile enemyProjectileScriptRight;

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


    }



    private void Update()
    {

        if (Player != null)
        {
            // make the ship face the player
            Vector3 directionToPlayerLW = Player.transform.position - leftWing.transform.position;
            float angleLW = Mathf.Atan2(directionToPlayerLW.y, directionToPlayerLW.x) * Mathf.Rad2Deg;
            leftWing.transform.rotation = Quaternion.AngleAxis(angleLW, Vector3.back);


            Vector3 directionToPlayerRW = Player.transform.position - rightWing.transform.position;
            float angleRW = Mathf.Atan2(directionToPlayerRW.y, directionToPlayerRW.x) * Mathf.Rad2Deg;
            rightWing.transform.rotation = Quaternion.AngleAxis(angleRW, Vector3.back);


        }


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

        if (leftInMotion)
        {
            Color targetColor = isLerpingForward ? travelColors[1] : travelColors[0];
            shipSpriteRenderer.color = Color.Lerp(travelColors[0], targetColor, colourTime);
            leftWingSpriteRenderer.color = Color.Lerp(travelColors[0], targetColor, colourTime);
            enemyProjectileScriptLeft.enabled = false;
        }
        else
        {
            Color targetColor = isLerpingForward ? idleColors[1] : idleColors[0];
            shipSpriteRenderer.color = Color.Lerp(idleColors[0], targetColor, colourTime);
            leftWingSpriteRenderer.color = Color.Lerp(idleColors[0], targetColor, colourTime);
            enemyProjectileScriptLeft.enabled = true;
        }

        if (rightInMotion)
        {
            Color targetColor = isLerpingForward ? travelColors[1] : travelColors[0];
            shipSpriteRenderer.color = Color.Lerp(travelColors[0], targetColor, colourTime);
            rightWingSpriteRenderer.color = Color.Lerp(travelColors[0], targetColor, colourTime);
            enemyProjectileScriptRight.enabled = false;
        }
        else
        {
            Color targetColor = isLerpingForward ? idleColors[1] : idleColors[0];
            shipSpriteRenderer.color = Color.Lerp(idleColors[0], targetColor, colourTime);
            rightWingSpriteRenderer.color = Color.Lerp(idleColors[0], targetColor, colourTime);
            enemyProjectileScriptRight.enabled = true;
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
            leftStartPosition = leftWing.transform.position;
            rightStartPosition = rightWing.transform.position;

            leftTargetPosition = targetPositions[UnityEngine.Random.Range(0, targetPositions.Length)].transform.position;
            rightTargetPosition = targetPositions[UnityEngine.Random.Range(0, targetPositions.Length)].transform.position;

            // Set the object's position to the start position
            leftWing.transform.position = leftStartPosition;
            rightWing.transform.position = rightStartPosition;

            // Start the coroutine to move the object to the target position
            leftInMotion = true;
            yield return StartCoroutine(MoveObject(leftWing, leftStartPosition, leftTargetPosition, durationInSeconds));
            leftInMotion = false;
            rightInMotion = true;
            yield return StartCoroutine(MoveObject(rightWing, rightStartPosition, rightTargetPosition, durationInSeconds));
            rightInMotion = false;
            // Wait for a short period before starting again
            yield return new WaitForSeconds(timeToWait);
        }

    }



    private IEnumerator MoveObject(GameObject objectToMove, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        // Calculate the elapsed time
        float elapsedTime = 0;

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
    }




}
