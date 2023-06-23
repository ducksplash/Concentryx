using UnityEngine;

public class BG : MonoBehaviour
{
    public Renderer backgroundRenderer;

    public float minContrast = 0.5f;
    public float maxContrast = 1.5f;

    public float changeDuration = 2f;

    private Material material;

    private float startContrast;
    private float targetContrast;

    private bool increasingContrast = true;

    // Material property ID for "_Contrast"
    private static readonly int ContrastPropertyID = Shader.PropertyToID("_Contrast");

    private float changeProgress;

    private void Start()
    {
        material = backgroundRenderer.material;

        startContrast = material.GetFloat(ContrastPropertyID);

        targetContrast = maxContrast;

        changeProgress = 1f / (changeDuration * 2f);

        StartCoroutine(ChangeContrast());
    }

    private System.Collections.IEnumerator ChangeContrast()
    {
        float changeTimer = 0f;

        while (true)
        {
            changeTimer += Time.smoothDeltaTime;

            // Calculate the current progress of the contrast change
            float t = Mathf.Clamp01(changeTimer * changeProgress);

            // Determine the current contrast based on whether it is increasing or decreasing
            float currentContrast;
            if (increasingContrast)
            {
                currentContrast = Mathf.Lerp(startContrast, targetContrast, t);
            }
            else
            {
                currentContrast = Mathf.Lerp(targetContrast, startContrast, t);
            }

            // Set the current contrast value in the material using the cached property ID
            material.SetFloat(ContrastPropertyID, currentContrast);

            // Check if the contrast change duration has been completed
            if (changeTimer >= (changeDuration * 2f)) // Double the duration for a full cycle
            {
                // Reverse the contrast change direction
                increasingContrast = !increasingContrast;

                changeTimer = 0f;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
