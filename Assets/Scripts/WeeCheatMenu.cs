using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeeCheatMenu : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas cheatButtonCanvas;

    public void Start()
    {
        mainCamera = Camera.main;

        cheatButtonCanvas.worldCamera = mainCamera;
    }

    public void CheatMenu()
    {
        if (GameMaster.instance.cheatMenuCanvas.alpha == 0)
        {
            GameMaster.instance.cheatMenuCanvas.alpha = 1;
            GameMaster.instance.cheatMenuCanvas.interactable = true;
            GameMaster.instance.cheatMenuCanvas.blocksRaycasts = true;
        }
        else
        {
            GameMaster.instance.cheatMenuCanvas.alpha = 0;
            GameMaster.instance.cheatMenuCanvas.interactable = false;
            GameMaster.instance.cheatMenuCanvas.blocksRaycasts = false;
        }
    }


}