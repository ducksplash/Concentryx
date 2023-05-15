using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PulsatingLight : MonoBehaviour
{
    [SerializeField] private float minIntensity = 0.3f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float speed = 1f;

    private Light2D light2D;

    private void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        light2D.intensity = intensity;
    }
}
