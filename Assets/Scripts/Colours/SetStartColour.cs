
using UnityEngine;
using UnityEngine.UI;

public class SetStartColour : MonoBehaviour
{
    public GameObject theStartColorPicker;
    private StartColorPicker thisStartColorPicker;

    private void Start()
    {

        var coltmp = new Color(0, 0, 0);
        if (PlayerPrefs.GetString("StartColor") != "")
        {
            ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("StartColor"), out coltmp);
        }
        else
        {
            coltmp = new Color(255, 255, 255);
        }

        thisStartColorPicker = theStartColorPicker.GetComponent<StartColorPicker>();

        thisStartColorPicker.onColorChanged += OnColorChanged;
    }
    public void OnColorChanged(Color c)
    {
        string colorString = ColorUtility.ToHtmlStringRGB(c);

        PlayerPrefs.SetString("StartColor", colorString);
        Debug.Log(colorString);
        Ship.instance.SetJetColors();
    }



    private void OnDestroy()
    {
        if (thisStartColorPicker != null)
            thisStartColorPicker.onColorChanged -= OnColorChanged;
    }
}