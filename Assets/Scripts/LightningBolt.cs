using UnityEngine;

public class LightningBolt
{
    public LineRenderer[] lineRenderer { get; set; }
    public LineRenderer lightRenderer { get; set; }

    public float SegmentLength { get; set; }
    public int Index { get; private set; }
    public bool IsActive { get; private set; }
    public GameObject gridParent { get; set; }

    public LightningBolt(float segmentLength, int index)
    {
        SegmentLength = segmentLength;
        Index = index;
    }

    public void Init(int lineRendererCount, GameObject lineRendererPrefab, GameObject lightRendererPrefab)
    {
        // Create the needed LineRenderer instances
        lineRenderer = new LineRenderer[lineRendererCount];
        for (int i = 0; i < lineRendererCount; i++)
        {
            lineRenderer[i] = GameObject.Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
            lineRenderer[i].gameObject.tag = "LightningBolt";
            lineRenderer[i].enabled = false;
            lineRenderer[i].transform.SetParent(gridParent.transform);

        }
        lightRenderer = GameObject.Instantiate(lightRendererPrefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
        lightRenderer.gameObject.tag = "LightningBolt";
        lightRenderer.gameObject.transform.SetParent(gridParent.transform);
        IsActive = false;
    }

    public void Activate()
    {
        // Activate this LightningBolt with all of its LineRenderers
        for (int i = 0; i < lineRenderer.Length; i++)
        {
            lineRenderer[i].enabled = true;
        }
        lightRenderer.enabled = true;
        IsActive = true;
    }

    public void DrawLightning(Vector2 source, Vector2 target)
    {
        // Check if the target is destroyed
        if (target == null)
        {
            Deactivate(); // Call the method to deactivate the lightning bolt
            return; // Exit the method
        }

        // Calculate the amount of segments
        float distance = Vector2.Distance(source, target);
        int segments = Mathf.Max(Mathf.FloorToInt(distance / SegmentLength) + 2, 4);

        for (int i = 0; i < lineRenderer.Length; i++)
        {
            // Set the amount of points to the calculated value
            lineRenderer[i].positionCount = segments;
            lineRenderer[i].SetPosition(0, source);
            Vector2 lastPosition = source;
            for (int j = 1; j < segments - 1; j++)
            {
                // Go linear from source to target
                Vector2 tmp = Vector2.Lerp(source, target, (float)j / (float)segments);
                // Add randomness
                lastPosition = new Vector2(tmp.x + Random.Range(-0.1f, 0.1f), tmp.y + Random.Range(-0.1f, 0.1f));
                // Set the calculated position
                lineRenderer[i].SetPosition(j, lastPosition);
            }
            lineRenderer[i].SetPosition(segments - 1, target);
        }

        // Set the points for the light
        lightRenderer.SetPosition(0, source);
        lightRenderer.SetPosition(1, target);

        // Set the color of the light
        Color lightColor = new Color(0.5647f, 0.58823f, 1f, Random.Range(0.2f, 1f));
        lightRenderer.startColor = lightColor;
        lightRenderer.endColor = lightColor;

        // Hide unused segments
        for (int i = segments; i < lineRenderer.Length; i++)
        {
            lineRenderer[i].enabled = false;
        }
    }


    public void Deactivate()
    {
        // Destroy all LineRenderer clones and disable the lightRenderer
        for (int i = 0; i < lineRenderer.Length; i++)
        {
            // Destroy the LineRenderer clone
            if (lineRenderer[i] != null)
            {
                lineRenderer[i].enabled = false;
                lineRenderer[i].gameObject.SetActive(false);
            }
        }
        // Disable the lightRenderer
        lightRenderer.enabled = false;
        IsActive = false;
    }

    public void DestroyLightning()
    {
        // Destroy all LineRenderer clones and disable the lightRenderer
        for (int i = 0; i < lineRenderer.Length; i++)
        {
            // Destroy the LineRenderer clone
            if (lineRenderer[i] != null)
            {
                lineRenderer[i].enabled = false;
                lineRenderer[i].gameObject.SetActive(false);
                GameObject.Destroy(lineRenderer[i].gameObject);
            }
        }
        // Disable the lightRenderer
        lightRenderer.enabled = false;
        GameObject.Destroy(lightRenderer.gameObject);
        IsActive = false;
    }

}
