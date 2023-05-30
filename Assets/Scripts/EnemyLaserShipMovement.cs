using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EnemyLaserShipMovement : MonoBehaviour
{

    public enum Orientation
    {
        Top,
        Bottom,
        Left,
        Right
    }

    private Orientation shipOrientation;
    public float durationInSeconds = 60f;
    public GameObject emenyLaserShip;
    public GameObject[] targetPositions;
    Vector3 startPosition;
    Vector3 targetPosition;
    private bool isRunning = false;
    Quaternion initialRotation;


    private void Start()
    {

        initialRotation = emenyLaserShip.transform.rotation;

        StartCoroutine(LaserShipRoutine());
    }

    private IEnumerator LaserShipRoutine()
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

        TakeSides();


        float totalDistance = (targetPosition - startPosition).magnitude;
        int numSteps = Mathf.RoundToInt(durationInSeconds * 300f);

        for (int step = 0; step <= numSteps; step++)
        {
            float t = (float)step / numSteps;
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Update the object's position
            emenyLaserShip.transform.localPosition = newPosition;

            // Wait for the next FixedUpdate
            yield return new WaitForFixedUpdate();
        }

        // Ensure the object reaches the target position exactly
        emenyLaserShip.transform.localPosition = targetPosition;

    }



    public void TakeSides()
    {


        switch (GetRandomEnumValue<Orientation>())
        {
            case Orientation.Top:
                emenyLaserShip.transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, 180f);
                emenyLaserShip.transform.localPosition = targetPositions[0].transform.localPosition;
                startPosition = emenyLaserShip.transform.localPosition;
                targetPosition = targetPositions[1].transform.localPosition;
                Debug.Log("Top");
                break;

            case Orientation.Bottom:
                emenyLaserShip.transform.rotation = initialRotation;
                emenyLaserShip.transform.localPosition = targetPositions[2].transform.localPosition;
                startPosition = emenyLaserShip.transform.localPosition;
                targetPosition = targetPositions[3].transform.localPosition;
                Debug.Log("Bottom");
                break;

            case Orientation.Left:
                emenyLaserShip.transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, 270f);
                emenyLaserShip.transform.localPosition = targetPositions[4].transform.localPosition;
                startPosition = emenyLaserShip.transform.localPosition;
                targetPosition = targetPositions[5].transform.localPosition;
                Debug.Log("Left");
                break;

            case Orientation.Right:
                emenyLaserShip.transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, 90f);
                emenyLaserShip.transform.localPosition = targetPositions[6].transform.localPosition;
                startPosition = emenyLaserShip.transform.localPosition;
                targetPosition = targetPositions[7].transform.localPosition;
                Debug.Log("Right");
                break;
        }


    }


    private T GetRandomEnumValue<T>()
    {
        T[] enumValues = (T[])System.Enum.GetValues(typeof(T));
        return enumValues[Random.Range(0, enumValues.Length)];
    }

}
