using UnityEngine;
using UnityEngine.EventSystems;

public class ThumbstickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public float maxThumbstickDistance = 100f; // Maximum distance the thumbstick can move from the center
    public Vector2 movement { get; private set; } // Current movement vector

    private RectTransform thumbstickRect;
    private Vector2 thumbstickStartPosition;

    private void Awake()
    {
        thumbstickRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Set the thumbstick start position
        thumbstickStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPosition = eventData.position;

        // Calculate the direction from the thumbstick start position to the current position
        Vector2 direction = currentPosition - thumbstickStartPosition;

        // Check if the pointer is within the thumbstick boundaries
        if (direction.magnitude > maxThumbstickDistance)
        {
            // Clamp the direction vector to the maximum distance
            direction = direction.normalized * maxThumbstickDistance;
        }

        // Update the thumbstick position
        thumbstickRect.anchoredPosition = direction;

        // Update the movement vector
        movement = direction / maxThumbstickDistance;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset the thumbstick position and movement vector
        thumbstickRect.anchoredPosition = Vector2.zero;
        movement = Vector2.zero;
    }
}
