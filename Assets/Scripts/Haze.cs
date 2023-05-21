using System.Collections;
using UnityEngine;

public class Haze : MonoBehaviour
{

    public Transform hazeTransform;
    public Material hazeMaterial;


    void Start()
    {
        hazeTransform = GetComponent<Transform>();

        hazeMaterial = GetComponent<Renderer>().material;

        StartCoroutine(HazeScale());
        StartCoroutine(RotateHaze());
        StartCoroutine(HazeEmission());
        StartCoroutine(MoveHaze(100f));

    }



    public IEnumerator HazeScale()
    {
        float scaleIncrement = 0.0001f;
        float maxScale = 1.4f;
        float minScale = 1.2f;

        while (true)
        {
            while (hazeTransform.localScale.x < maxScale)
            {
                hazeTransform.localScale += new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(0.5f); // Delay at max scale

            while (hazeTransform.localScale.x > minScale)
            {
                hazeTransform.localScale -= new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(0.5f); // Delay at min scale
        }
    }


    public IEnumerator RotateHaze()
    {
        float rotationSpeed = 0.3f; // Adjust the speed of rotation as desired

        while (true)
        {
            hazeTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }



    public IEnumerator MoveHaze(float duration)
    {
        float moveSpeedDown = 0.05f; // Adjust the downward speed as desired
        float moveSpeedRight = 0.01f; // Adjust the rightward speed as desired

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


    public IEnumerator HazeEmission()
    {
        float intensityIncrement = 0.001f;
        float maxIntensity = 1.6f;
        float minIntensity = 0.2f;
        Color pinkColor = Color.red;
        Color blueColor = Color.blue;

        Material hazeMaterial = GetComponent<Renderer>().material; // Assuming the material is on the same game object as the script

        while (true)
        {
            while (hazeMaterial.GetColor("_EmissionColor").maxColorComponent < maxIntensity)
            {
                Color currentColor = hazeMaterial.GetColor("_EmissionColor");
                float newIntensity = currentColor.maxColorComponent + intensityIncrement;
                float t = (newIntensity - minIntensity) / (maxIntensity - minIntensity);
                Color newColor = Color.Lerp(pinkColor, blueColor, t);

                hazeMaterial.SetColor("_EmissionColor", newColor);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(0.5f); // Delay at max intensity

            while (hazeMaterial.GetColor("_EmissionColor").maxColorComponent > minIntensity)
            {
                Color currentColor = hazeMaterial.GetColor("_EmissionColor");
                float newIntensity = currentColor.maxColorComponent - intensityIncrement;
                float t = (newIntensity - minIntensity) / (maxIntensity - minIntensity);
                Color newColor = Color.Lerp(blueColor, pinkColor, t);
                hazeMaterial.SetColor("_EmissionColor", newColor);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(0.5f); // Delay at min intensity
        }
    }




}