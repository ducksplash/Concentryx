using System.Collections;
using UnityEngine;

public class Haze : MonoBehaviour
{
    public Transform hazeTransform;
    public Material hazeMaterial;

    public Vector3 startingPosition;

    public Coroutine hazeScaleCoroutine = null;
    public Coroutine hazeRotationCoroutine = null;
    public Coroutine hazeEmissionCoroutine = null;
    public Coroutine hazeMovementCoroutine = null;

    public float radius = 2f;

    private void Start()
    {
        MakeHazed();
    }

    public void MakeHazed()
    {
        // Get the starting position

        // Generate a random direction and distance within the specified radius
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        float randomDistance = Random.Range(4f, radius);

        // Calculate the new position based on the starting position, random direction, and random distance
        Vector3 newPosition = startingPosition + randomDirection * randomDistance;

        // Set the new position for the object
        hazeTransform.localPosition = newPosition;

        MakeHazeWaste();

        hazeScaleCoroutine = StartCoroutine(HazeScale());
        hazeRotationCoroutine = StartCoroutine(RotateHaze());
        hazeEmissionCoroutine = StartCoroutine(HazeEmission());
        hazeMovementCoroutine = StartCoroutine(MoveHaze(100f));
    }


    public void MakeHazeWaste()
    {
        // stop them if they're started

        if (hazeScaleCoroutine != null)
        {
            StopCoroutine(hazeScaleCoroutine);
            hazeScaleCoroutine = null;
        }

        if (hazeRotationCoroutine != null)
        {
            StopCoroutine(hazeRotationCoroutine);
            hazeRotationCoroutine = null;
        }

        if (hazeEmissionCoroutine != null)
        {
            StopCoroutine(hazeEmissionCoroutine);
            hazeEmissionCoroutine = null;
        }

        if (hazeMovementCoroutine != null)
        {
            StopCoroutine(hazeMovementCoroutine);
            hazeMovementCoroutine = null;
        }
    }

    private IEnumerator HazeScale()
    {
        const float scaleIncrement = 0.0001f;
        const float minScale = 0.8f;
        const float maxScale = 0.9f;
        const float delay = 0.1f;

        Vector3 scaleIncrementVector = new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);
        WaitForSeconds delayWait = new WaitForSeconds(delay);

        while (true)
        {
            while (hazeTransform.localScale.x < maxScale)
            {
                hazeTransform.localScale += scaleIncrementVector * Time.deltaTime;
                yield return delayWait;
            }

            yield return delayWait; // Delay at max scale

            while (hazeTransform.localScale.x > minScale)
            {
                hazeTransform.localScale -= scaleIncrementVector * Time.deltaTime;
                yield return delayWait;
            }

            yield return delayWait; // Delay at min scale
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
        float moveSpeedDown = UnityEngine.Random.Range(-0.06f, 0.06f);
        float moveSpeedRight = UnityEngine.Random.Range(-0.02f, 0.02f);

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
        float intensityIncrement = UnityEngine.Random.Range(-0.02f, 0.02f);
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

            yield return new WaitForSeconds(0.5f); // Delay at max intensity

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

            yield return new WaitForSeconds(0.5f); // Delay at min intensity
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
