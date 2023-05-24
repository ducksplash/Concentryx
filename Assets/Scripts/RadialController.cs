using UnityEngine;
using UnityEngine.EventSystems;

public class RadialController : MonoBehaviour, IDragHandler
{
    private Vector2 startDragPosition;
    private float rotationDamping = 4f; // Adjust the damping factor as needed

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPosition = eventData.position;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(), currentPosition, eventData.pressEventCamera, out Vector2 localPosition))
        {
            float angle = Vector2.SignedAngle(startDragPosition - (Vector2)transform.localPosition, localPosition - (Vector2)transform.localPosition);
            float smoothedAngle = Mathf.LerpAngle(0f, angle, Time.deltaTime * rotationDamping);
            transform.Rotate(0f, 0f, smoothedAngle);
            startDragPosition = localPosition;
            Ship.instance.Rotate(smoothedAngle);
            Debug.Log("Rotation angle: " + smoothedAngle);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
    }
}
