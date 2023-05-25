using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickBack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool ClickBackEnabled;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ClickBackEnabled)
        {
            Debug.Log(eventData.ToString());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ClickBackEnabled)
        {
            Debug.Log(eventData.ToString());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (ClickBackEnabled)
        {
            Debug.Log(eventData.ToString());
        }
    }
}



