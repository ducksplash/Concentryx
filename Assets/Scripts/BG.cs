// Importing the necessary Unity Engine library
using UnityEngine;

// Class representing the background object
public class BG : MonoBehaviour
{
    // Reference to the background renderer component
    public Renderer backgroundRenderer;

    // Minimum and maximum contrast values
    public float minContrast = 0.5f;
    public float maxContrast = 1.5f;

    // Duration of the contrast change
    public float changeDuration = 2f;

    // Material used for rendering the background
    private Material material;

    // Starting and target contrast values
    private float startContrast;
    private float targetContrast;

    // Flag indicating whether the contrast is increasing or decreasing
    private bool increasingContrast = true;

    // Called when the object is first created
    private void Start()
    {
        // Get the material component from the background renderer
        material = backgroundRenderer.material;

        // Store the initial contrast value
        startContrast = material.GetFloat("_Contrast");

        // Set the target contrast value to the maximum contrast
        targetContrast = maxContrast;

        // Start the coroutine for changing the contrast
        StartCoroutine(ChangeContrast());
    }

    // Coroutine for changing the contrast gradually over time
    private System.Collections.IEnumerator ChangeContrast()
    {
        // Timer for tracking the duration of the contrast change
        float changeTimer = 0f;

        // Loop continuously
        while (true)
        {
            // Increase the timer based on the elapsed time
            changeTimer += Time.smoothDeltaTime;

            // Calculate the progress of the contrast change (between 0 and 1)
            float t = Mathf.Clamp01(changeTimer / (changeDuration * 2f)); // Double the duration for a full cycle

            // Variable for storing the current contrast value
            float currentContrast;

            // Determine the current contrast based on whether it is increasing or decreasing
            if (increasingContrast)
            {
                currentContrast = Mathf.Lerp(startContrast, targetContrast, t);
            }
            else
            {
                currentContrast = Mathf.Lerp(targetContrast, startContrast, t);
            }

            // Set the current contrast value in the material
            material.SetFloat("_Contrast", currentContrast);

            // Check if the contrast change duration has been completed
            if (changeTimer >= (changeDuration * 2f)) // Double the duration for a full cycle
            {
                // Reverse the contrast change direction
                increasingContrast = !increasingContrast;

                // Reset the timer
                changeTimer = 0f;
            }

            // Pause the execution of the coroutine and resume in the next frame
            yield return null;
        }
    }
}
