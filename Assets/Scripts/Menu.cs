using UnityEngine;

public class Menu : MonoBehaviour
{



    public GameObject GameMenu;
    public CanvasGroup MainMenuCanvas;
    public CanvasGroup PauseMenuCanvas;



    void Start()
    {
        MainMenuCanvas.alpha = 0;
        MainMenuCanvas.blocksRaycasts = false;
        PauseMenuCanvas.alpha = 0;
        PauseMenuCanvas.blocksRaycasts = false;
    }






    public void Resume()
    {
        Debug.Log("Resume");
        ChangePage();
        Time.timeScale = 1f;
    }


    public void Pause()
    {
        Debug.Log("Pause");
        ChangePage(PauseMenuCanvas);

        Time.timeScale = 0f;

    }


    public void ChangePage(CanvasGroup ToOpen = null)
    {
        CanvasGroup[] canvasGroups = GameMenu.GetComponentsInChildren<CanvasGroup>();

        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }

        if (ToOpen != null)
        {
            ToOpen.alpha = 1f;
            ToOpen.blocksRaycasts = true;
        }
    }


}