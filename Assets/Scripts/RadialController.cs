using UnityEngine;
using UnityEngine.EventSystems;

public class RadialController : MonoBehaviour, IDragHandler
{
    private Vector2 startDragPosition;
    public float rotationDamping = 4f; // Adjust the damping factor as needed

    public float speedModifier = 1f;


    public void OnDrag(PointerEventData eventData)
    {
        if (GameMaster.instance.onMobile)
        {
            Vector2 currentPosition = eventData.position;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(), currentPosition, eventData.pressEventCamera, out Vector2 localPosition))
            {
                float angle = Vector2.SignedAngle(startDragPosition - (Vector2)transform.localPosition, localPosition - (Vector2)transform.localPosition);
                float smoothedAngle = Mathf.LerpAngle(0f, angle, Time.deltaTime * rotationDamping);
                transform.Rotate(0f, 0f, smoothedAngle * speedModifier);
                startDragPosition = localPosition;
                Ship.instance.Rotate(smoothedAngle * speedModifier);
            }
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
    }
}
