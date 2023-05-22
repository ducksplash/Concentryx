using System.Collections;
using UnityEngine;

public class Haze : MonoBehaviour
{
    public Transform hazeTransform;
    public Material hazeMaterial;

    private void Start()
    {
        hazeTransform = transform;
        hazeMaterial = GetComponent<Renderer>().material;

        StartCoroutine(HazeScale());
        StartCoroutine(RotateHaze());
        StartCoroutine(HazeEmission());
        StartCoroutine(MoveHaze(100f));
    }

    private IEnumerator HazeScale()
    {
        const float scaleIncrement = 0.0001f;
        const float minScale = 0.8f;
        const float maxScale = 0.9f;
        const float delay = 0.1f;

        while (true)
        {
            while (hazeTransform.localScale.x < maxScale)
            {
                yield return new WaitForSeconds(delay);
                hazeTransform.localScale += new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);
                yield return null;
            }

            yield return new WaitForSeconds(delay); // Delay at max scale

            while (hazeTransform.localScale.x > minScale)
            {
                yield return new WaitForSeconds(delay);
                hazeTransform.localScale -= new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);
                yield return null;
            }

            yield return new WaitForSeconds(delay); // Delay at min scale
        }
    }

    private IEnumerator RotateHaze()
    {
        const float rotationSpeed = 0.1f;

        while (true)
        {
            hazeTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveHaze(float duration)
    {
        const float moveSpeedDown = 0.06f;
        const float moveSpeedRight = 0.02f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float moveDistanceDown = moveSpeedDown * Time.deltaTime;
            float moveDistanceRight = moveSpeedRight * Time.deltaTime;

            hazeTransform.Translate(new Vector3(moveDistanceRight, -moveDistanceDown, 0f));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator HazeEmission()
    {
        const float intensityIncrement = 0.001f;
        const float maxIntensity = 1.8f;
        const float minIntensity = 0.2f;
        Color pinkColor = Color.red;
        Color blueColor = Color.blue;

        Color startColor = pinkColor;
        Color endColor = blueColor;

        while (true)
        {
            while (hazeMaterial.GetColor("_EmissionColor").maxColorComponent < maxIntensity)
            {
                Color currentColor = hazeMaterial.GetColor("_EmissionColor");
                float newIntensity = currentColor.maxColorComponent + intensityIncrement;
                float t = (newIntensity - minIntensity) / (maxIntensity - minIntensity);
                Color newColor = Color.Lerp(startColor, endColor, t);

                hazeMaterial.SetColor("_EmissionColor", newColor);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Delay at max intensity

            startColor = blueColor;
            endColor = pinkColor;

            while (hazeMaterial.GetColor("_EmissionColor").maxColorComponent > minIntensity)
            {
                Color currentColor = hazeMaterial.GetColor("_EmissionColor");
                float newIntensity = currentColor.maxColorComponent - intensityIncrement;
                float t = (newIntensity - minIntensity) / (maxIntensity - minIntensity);
                Color newColor = Color.Lerp(startColor, endColor, t);

                hazeMaterial.SetColor("_EmissionColor", newColor);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Delay at min intensity
        }
    }
}
