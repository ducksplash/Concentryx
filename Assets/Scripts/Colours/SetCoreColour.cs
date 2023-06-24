
using UnityEngine;
using UnityEngine.UI;

public class SetCoreColour : MonoBehaviour
{
    public Material CoreMaterial;

    public GameObject theCoreColorPickerObject;
    private CoreColorPicker thisCoreColorPicker;

    private void Start()
    {



        var coltmp = new Color(0, 0, 0);
        if (PlayerPrefs.GetString("CoreColor") != "")
        {
            ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("CoreColor"), out coltmp);
        }
        else
        {
            coltmp = new Color(255, 255, 255);
        }

        thisCoreColorPicker = theCoreColorPickerObject.GetComponent<CoreColorPicker>();

        thisCoreColorPicker.onColorChanged += OnColorChanged;
    }

    public void OnColorChanged(Color c)
    {

        string colorString = ColorUtility.ToHtmlStringRGB(c);

        PlayerPrefs.SetString("CoreColor", colorString);

    }

}