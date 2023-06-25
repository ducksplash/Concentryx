using System.Collections;
using UnityEngine;

public class Haze : MonoBehaviour
{
    public Transform hazeTransform;
    public Material hazeMaterial;

    public Vector3 startingPosition;
    public float radius = 2f;

    private void Start()
    {
        MakeHazed();
    }

    public void MakeHazed()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        float randomDistance = Random.Range(4f, radius);
        Vector3 newPosition = startingPosition + randomDirection * randomDistance;
        hazeTransform.localPosition = newPosition;

        MakeHazeWaste();

        StartCoroutine(HazeScale());
        StartCoroutine(RotateHaze());
        StartCoroutine(HazeEmission());
        StartCoroutine(MoveHaze(100f));
    }

    public void MakeHazeWaste()
    {
        StopAllCoroutines();
    }

    private IEnumerator HazeScale()
    {
        float scaleIncrement = 0.0001f;
        float minScale = 0.8f;
        float maxScale = 0.9f;

        Vector3 scaleIncrementVector = new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

        while (true)
        {
            while (hazeTransform.localScale.x < maxScale)
            {
                hazeTransform.localScale += scaleIncrementVector * Time.deltaTime;
                yield return null;
            }

            while (hazeTransform.localScale.x > minScale)
            {
                hazeTransform.localScale -= scaleIncrementVector * Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator RotateHaze()
    {
        float rotationSpeed = UnityEngine.Random.Range(-0.1f, 0.3f);

        while (true)
        {
            hazeTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveHaze(float duration)
    {
        float moveSpeed = UnityEngine.Random.Range(-0.06f, 0.06f);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            hazeTransform.Translate(new Vector3(moveSpeed, -moveSpeed, 0f) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator HazeEmission()
    {
        float intensityIncrement = 0.02f;
        float maxIntensity = UnityEngine.Random.Range(1.6f, 1.9f);
        float minIntensity = UnityEngine.Random.Range(0.15f, 0.25f);
        Color pinkColor = GetRandomColor();
        Color blueColor = GetRandomColor();
        Color startColor = GetRandomColor();
        Color endColor = GetRandomColor();
        float t = 0f;

        while (true)
        {
            while (hazeMaterial.GetColor("_EmissionColor").maxColorComponent < maxIntensity)
            {
                t += intensityIncrement / (maxIntensity - minIntensity);
                Color newColor = Color.Lerp(startColor, endColor, t);

                hazeMaterial.SetColor("_EmissionColor", newColor);
                yield return null;
            }

            startColor = blueColor;
            endColor = pinkColor;
            t = 0f;

            while (hazeMaterial.GetColor("_EmissionColor").maxColorComponent > minIntensity)
            {
                t += intensityIncrement / (maxIntensity - minIntensity);
                Color newColor = Color.Lerp(startColor, endColor, t);

                hazeMaterial.SetColor("_EmissionColor", newColor);
                yield return null;
            }
        }
    }

    public Color GetRandomColor()
    {
        float r = Random.value;
        float g = Random.value;
        float b = Random.value;
        return new Color(r, g, b);
    }
}
