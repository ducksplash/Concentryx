using UnityEngine;

public class CaterpillarMovement : MonoBehaviour
{
    public Transform[] segments; // Array of caterpillar segments
    public Transform[] targets; // Array of target positions for each segment
    public float moveSpeed = 2f; // Speed of caterpillar movement
    public float segmentSpacing = 0.5f; // Spacing between segments

    private int currentIndex = 0; // Current target index

    private void Start()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

    }

    private void Update()
    {
        // Check if any segments are missing and reorganize the remaining segments
        for (int i = 1; i < segments.Length; i++)
        {
            if (segments[i] == null)
            {
                // Remove the missing segment from the array
                RemoveSegment(i);
                return; // Exit the update loop if the lead segment is missing
            }

            Transform currentSegment = segments[i];
            Transform prevSegment = segments[i - 1];

            if (prevSegment == null)
            {
                // If the previous segment is missing, move the current segment directly towards the target
                currentSegment.position = Vector3.MoveTowards(currentSegment.position, targets[currentIndex].position, moveSpeed * Time.deltaTime);
            }
            else
            {
                // Calculate the target position based on the previous segment's position
                Vector3 targetPosition = prevSegment.position - (prevSegment.position - currentSegment.position).normalized * segmentSpacing;
                currentSegment.position = Vector3.MoveTowards(currentSegment.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }

        // Move the head segment towards the current target
        if (segments.Length > 0)
        {
            Transform headSegment = segments[0];
            if (headSegment == null)
            {
                // Find the next non-null segment and make it the head segment
                for (int i = 1; i < segments.Length; i++)
                {
                    if (segments[i] != null)
                    {
                        headSegment = segments[i];
                        segments[0] = headSegment;
                        break;
                    }
                }
            }

            if (headSegment != null)
            {
                headSegment.position = Vector3.MoveTowards(
                    headSegment.position,
                    targets[currentIndex].position,
                    moveSpeed * Time.deltaTime
                );

                // Check if the head segment has reached the current target
                if (headSegment.position == targets[currentIndex].position)
                {
                    // Move to the next target index
                    currentIndex = (currentIndex + 1) % targets.Length;
                }
            }
        }
    }

    private void RemoveSegment(int index)
    {
        if (index < 0 || index >= segments.Length)
            return;

        // Destroy the missing segment
        // Destroy(segments[index].gameObject);

        // Shift the remaining segments in the array
        for (int i = index; i < segments.Length - 1; i++)
        {
            segments[i] = segments[i + 1];
        }

        // Resize the array
        System.Array.Resize(ref segments, segments.Length - 1);
    }
}
