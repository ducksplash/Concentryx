using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public GameObject rotateAroundObject;
    public float rotationSpeed = 20f;
    public float minChangeDirectionInterval = 2f;
    public float maxChangeDirectionInterval = 8f;

    private float directionTimer;
    private bool rotateClockwise;

    private void Start()
    {
        ResetDirectionTimer();
        rotateClockwise = true;
    }

    private void Update()
    {
        // Rotate the sprite around the specified object
        transform.RotateAround(rotateAroundObject.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

        // Update the timer
        directionTimer -= Time.deltaTime;

        // Check if it's time to change rotation direction
        if (directionTimer <= 0f)
        {
            // Randomly determine the new rotation direction
            rotateClockwise = Random.value < 0.5f;

            // Reset the timer with a random interval between minChangeDirectionInterval and maxChangeDirectionInterval
            ResetDirectionTimer();
        }

        // Set the rotation direction based on the current setting
        rotationSpeed = rotateClockwise ? Mathf.Abs(rotationSpeed) : -Mathf.Abs(rotationSpeed);
    }

    private void ResetDirectionTimer()
    {
        directionTimer = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval);
    }
}
