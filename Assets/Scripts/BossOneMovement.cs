using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BossOneMovement : MonoBehaviour
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

    public Color[] travelColors;
    public Color[] idleColors;
    public GameObject bossShip;
    public GameObject[] targetPositions;
    Vector3 startPosition;
    Vector3 targetPosition;
    private bool isRunning = false;

    public GameObject Player;
    Quaternion initialRotation;

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

    }



    private void Update()
    {
        if (Player != null)
        {
            Vector3 directionToPlayer = Player.transform.position - bossShip.transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            bossShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }


    private IEnumerator BossBoatRoutine()
    {
        while (true)
        {
            // Check if the coroutine is already running
            if (!isRunning)
            {
                // Set the flag to indicate that the coroutine is running
                isRunning = true;

                // Start the sub-coroutine
                yield return StartCoroutine(ShipFloatAway());

                // Reset the flag to indicate that the coroutine has finished
                isRunning = false;
            }

            // Wait for a short period before starting again
            yield return new WaitForSeconds(1f);
        }
    }


    private IEnumerator ShipFloatAway()
    {


        // go to a target from the list
        // wait at target for a second
        // move to next target
        // and on.

        for (int i = 0; i < targetPositions.Length; i++)
        {
            startPosition = bossShip.transform.position;
            targetPosition = targetPositions[i].transform.position;

            // Set the object's position to the start position
            bossShip.transform.position = startPosition;

            // Start the coroutine to move the object to the target position
            yield return StartCoroutine(MoveObject(bossShip, startPosition, targetPosition, durationInSeconds));

            // Wait for a short period before starting again
            yield return new WaitForSeconds(1f);
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
