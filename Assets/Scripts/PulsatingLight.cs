using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class PulsatingLight : MonoBehaviour
{
    [SerializeField] private float minIntensity = 0.3f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float speed = 1f;

    private UnityEngine.Rendering.Universal.Light2D light2D;
    private Coroutine pulsateCoroutine;

    private void Start()
    {
        light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        StartPulsating();
    }

    private void OnDestroy()
    {
        StopPulsating();
    }

    private void StartPulsating()
    {
        if (pulsateCoroutine == null)
        {
            pulsateCoroutine = StartCoroutine(Pulsate());
        }
    }

    private void StopPulsating()
    {
        if (pulsateCoroutine != null)
        {
            StopCoroutine(pulsateCoroutine);
            pulsateCoroutine = null;
        }
    }

    private IEnumerator Pulsate()
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f);
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
            light2D.intensity = intensity;
            yield return null; // Wait for the next frame
        }
    }
}
