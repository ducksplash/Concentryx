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

    private void Start()
    {
        material = backgroundRenderer.material;
        startContrast = material.GetFloat("_Contrast");
        targetContrast = maxContrast;

        StartCoroutine(ChangeContrast());
    }

    private System.Collections.IEnumerator ChangeContrast()
    {
        float changeTimer = 0f;

        while (true)
        {
            changeTimer += Time.deltaTime;

            float t = Mathf.Clamp01(changeTimer / (changeDuration * 2f)); // Double the duration for a full cycle
            float currentContrast;

            if (increasingContrast)
            {
                currentContrast = Mathf.Lerp(startContrast, targetContrast, t);
            }
            else
            {
                currentContrast = Mathf.Lerp(targetContrast, startContrast, t);
            }

            material.SetFloat("_Contrast", currentContrast);

            if (changeTimer >= (changeDuration * 2f)) // Double the duration for a full cycle
            {
                increasingContrast = !increasingContrast;
                changeTimer = 0f;
            }

            yield return null;
        }
    }
}
