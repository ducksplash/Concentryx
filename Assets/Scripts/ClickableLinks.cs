using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


[RequireComponent(typeof(TMP_Text))]
public class ClickableLinks : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        TMP_Text pTextMeshPro = GetComponent<TMP_Text>();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, Camera.main);
        if (linkIndex != -1)
        { // was a link clicked?
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }

}