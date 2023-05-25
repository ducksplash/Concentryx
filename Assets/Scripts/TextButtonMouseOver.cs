using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButtonMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private TextMeshProUGUI theText;
    public Color OrigColor = Color.white;
    public Color HoverColor = Color.black;


    public void Start()
    {
        theText = gameObject.GetComponent<TextMeshProUGUI>();

        OrigColor = theText.color;
        theText.color = OrigColor;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = HoverColor;
        theText.fontStyle = FontStyles.Underline;

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = OrigColor;
        theText.fontStyle = FontStyles.Normal;
    }
}



