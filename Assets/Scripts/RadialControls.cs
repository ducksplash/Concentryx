using UnityEngine;
using UnityEngine.EventSystems;

public class RadialControls : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform handle; // Reference to the RectTransform of the handle
    public CircleCollider2D radialCollider; // Reference to the Collider of the radial control

    private bool isDragging = false; // Flag to track if the handle is being dragged

    private Vector2 rotationOrigin; // Rotation origin point

    private void Start()
    {
        // Calculate the rotation origin as the center of the game object
        rotationOrigin = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Check if the mouse is over the radial control
        if (IsMouseOverRadialControl(eventData.position))
        {
            // Set the dragging flag to true
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // Convert the mouse position to world space
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

            // Get the direction from the rotation origin to the mouse position
            Vector2 handleDirection = mouseWorldPosition - (Vector3)rotationOrigin;

            // Calculate the distance from the rotation origin to the mouse position
            float handleDistance = handleDirection.magnitude;

            // Calculate the clamped distance within the bounds of the circle collider
            float clampedDistance = Mathf.Clamp(handleDistance, radialCollider.radius * 0.1f, radialCollider.radius);

            // Calculate the clamped position based on the clamped distance
            Vector2 clampedPosition = rotationOrigin + handleDirection.normalized * clampedDistance;

            // Set the handle position to the clamped position
            handle.position = clampedPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset the dragging flag to false
        isDragging = false;
    }

    private bool IsMouseOverRadialControl(Vector2 mousePosition)
    {
        // Convert the mouse position to world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Check if the world position is within the circle collider
        return radialCollider.OverlapPoint(mouseWorldPosition);
    }
}
