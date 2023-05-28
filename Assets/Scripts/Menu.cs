using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using RDG; // vibrate plugin
using UnityEngine.Audio;


public class Menu : MonoBehaviour
{



    public GameObject GameMenu;
    public CanvasGroup MainMenuCanvas;
    public CanvasGroup PauseMenuCanvas;
    public CanvasGroup RankMenuCanvas;
    public CanvasGroup SettingsMenuCanvas;
    public CanvasGroup SureMenuCanvas;
    public CanvasGroup XPSureCanvas;

    public CanvasGroup ThumbStickCanvas;

    // sfx
    public Slider SFXSlider;
    public TextMeshProUGUI SFXValueText;
    // sfx
    public Slider BGMSlider;
    public TextMeshProUGUI BGMValueText;

    public Toggle VibrationToggle;
    public Toggle PostProcessingToggle;

    public bool postProcessingEnabled;

    public AudioSource menuNoiseBox;
    public AudioClip[] menuNoises;
    public AudioMixer AudioMixer;

    public GameObject postProcessingVolume; // Reference to the Volume component

    public float minVolume = -80f; // Minimum volume in decibels

    void Start()
    {



        // Set initial toggle value based on the device vibration setting
        if (!PlayerPrefs.HasKey("PlayerVibration"))
        {
            GameMaster.instance.deviceVibrationEnabled = true;
            PlayerPrefs.SetInt("PlayerVibration", 1); // Set the value to true (1)
            PlayerPrefs.Save(); // Save the changes
        }
        else
        {
            int vibrationEnabled = PlayerPrefs.GetInt("PlayerVibration");
            GameMaster.instance.deviceVibrationEnabled = vibrationEnabled == 1 ? true : false;
        }



        // Set initial toggle value based on the post-processing setting
        if (!PlayerPrefs.HasKey("PostProcessingEnabled"))
        {

            postProcessingEnabled = true; // Save the changes

        }
        else
        {

            postProcessingEnabled = PlayerPrefs.GetInt("PostProcessingEnabled") == 1 ? true : false;

        }



        // Set the initial state of the vibration toggle
        VibrationToggle.isOn = GameMaster.instance.deviceVibrationEnabled;

        // Set the initial state of the post-processing toggle
        PostProcessingToggle.isOn = postProcessingEnabled;


        postProcessingVolume.SetActive(postProcessingEnabled);

        // Set initial slider value based on the SFX volume
        float SFXvolume = PlayerPrefs.GetFloat("SFXVolume", 0f);
        SFXvolume = Mathf.Clamp(SFXvolume, minVolume, 0f);
        float normalizedSFXVolume = Mathf.InverseLerp(minVolume, 0f, SFXvolume);
        int displayedSFXVolume = Mathf.RoundToInt(normalizedSFXVolume * 10f);

        SFXSlider.value = normalizedSFXVolume;
        SFXValueText.text = displayedSFXVolume.ToString();

        // Set initial slider value based on the BGM volume
        float BGMvolume = PlayerPrefs.GetFloat("BGMVolume", 0f);
        BGMvolume = Mathf.Clamp(BGMvolume, minVolume, 0f);
        float normalizedBGMVolume = Mathf.InverseLerp(minVolume, 0f, BGMvolume);
        int displayedBGMVolume = Mathf.RoundToInt(normalizedBGMVolume * 10f);

        BGMSlider.value = normalizedBGMVolume;
        BGMValueText.text = displayedBGMVolume.ToString();

        ChangePage(null, true);
    }

    public void TogglePostProcessing()
    {
        // Toggle the post-processing volume
        postProcessingVolume.SetActive(postProcessingVolume.activeSelf ? false : true);

        // Save the updated post-processing state in PlayerPrefs
        PlayerPrefs.SetInt("PostProcessingEnabled", postProcessingVolume.activeSelf ? 1 : 0);
        PlayerPrefs.Save();
    }


    public void ToggleVibration()
    {
        if (GameMaster.instance.deviceVibrationEnabled)
        {
            PlayerPrefs.SetInt("PlayerVibration", 0);  // Set the value to false (0)
            PlayerPrefs.Save();
            GameMaster.instance.deviceVibrationEnabled = false;
        }
        else
        {
            PlayerPrefs.SetInt("PlayerVibration", 1);  // Set the value to true (1)
            PlayerPrefs.Save();
            GameMaster.instance.deviceVibrationEnabled = true;
        }
    }


    //////////
    // sound effects
    public void OnSFXSliderValueChanged()
    {
        float volume = Mathf.Lerp(minVolume, 0f, SFXSlider.value);
        float convertedVolume = Mathf.Pow(10f, volume / 20f); // Convert volume from decibels to linear scale
        AudioMixer.SetFloat("SFX", Mathf.Log10(convertedVolume) * 20f); // Convert volume back to decibels

        float normalizedVolume = Mathf.InverseLerp(minVolume, 0f, volume);
        int displayedVolume = Mathf.RoundToInt(normalizedVolume * 10f); // Convert normalized volume to a value between 0 and 10
        SFXValueText.text = displayedVolume.ToString();

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();

    }

    //////////
    // background music
    public void OnBGMSliderValueChanged()
    {
        float volume = Mathf.Lerp(minVolume, 0f, BGMSlider.value);
        float convertedVolume = Mathf.Pow(10f, volume / 20f); // Convert volume from decibels to linear scale
        AudioMixer.SetFloat("BGM", Mathf.Log10(convertedVolume) * 20f); // Convert volume back to decibels

        float normalizedVolume = Mathf.InverseLerp(minVolume, 0f, volume);
        int displayedVolume = Mathf.RoundToInt(normalizedVolume * 10f); // Convert normalized volume to a value between 0 and 10
        BGMValueText.text = displayedVolume.ToString();

        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();

    }




    public void Resume()
    {
        TriggerVibrate();
        ChangePage();
        Time.timeScale = 1f;
    }

    public void RankScreen()
    {
        ChangePage(RankMenuCanvas);
        Time.timeScale = 0f;
    }


    public void SettingsScreen()
    {
        ChangePage(SettingsMenuCanvas);
        Time.timeScale = 0f;
    }


    public void Pause()
    {
        ChangePage(PauseMenuCanvas);

        Time.timeScale = 0f;

    }
    public void AreYouSure()
    {
        ChangePage(SureMenuCanvas);

        Time.timeScale = 0f;
    }
    public void XPSure()
    {
        ChangePage(XPSureCanvas);

        Time.timeScale = 0f;
    }

    public void ResetXP()
    {
        // Quit the game
        GameMaster.instance.ResetRank();
        ChangePage(RankMenuCanvas);

    }


    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }

    public void ChangePage(CanvasGroup ToOpen = null, bool FirstRun = false)
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
            ThumbStickCanvas.alpha = 0f;
            ThumbStickCanvas.blocksRaycasts = false;
            if (!FirstRun)
            {
                menuNoiseBox.clip = menuNoises[0];
                menuNoiseBox.PlayOneShot(menuNoises[0]);
            }
        }
        else
        {
            ThumbStickCanvas.alpha = 1f;
            ThumbStickCanvas.blocksRaycasts = true;
            if (!FirstRun)
            {
                menuNoiseBox.clip = menuNoises[1];
                menuNoiseBox.PlayOneShot(menuNoises[1]);
            }
        }
        TriggerVibrate();
    }









    public void TriggerVibrate()
    {
        Vibration.Vibrate(50, 255);
    }

}