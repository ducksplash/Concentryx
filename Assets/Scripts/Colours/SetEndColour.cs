using UnityEngine;
using UnityEngine.UI;

public class SetEndColour : MonoBehaviour
{

    public GameObject theEndColorPicker;
    private EndColorPicker thisEndColorPicker;

    private void Start()
    {

        var coltmp = new Color(0, 0, 0);
        if (PlayerPrefs.GetString("EndColor") != "")
        {
            ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("EndColor"), out coltmp);
        }
        else
        {
            coltmp = new Color(255, 255, 255);
        }



        thisEndColorPicker = theEndColorPicker.GetComponent<EndColorPicker>();

        thisEndColorPicker.onColorChanged += OnColorChanged;
    }

    public void OnColorChanged(Color c)
    {
        string colorString = ColorUtility.ToHtmlStringRGB(c);

        PlayerPrefs.SetString("EndColor", colorString);
        Debug.Log(colorString);
        Ship.instance.SetJetColors();
    }



    private void OnDestroy()
    {
        if (thisEndColorPicker != null)
            thisEndColorPicker.onColorChanged -= OnColorChanged;
    }
}