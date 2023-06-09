using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using RDG; // vibrate plugin
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Menu : MonoBehaviour
{
    public GameObject GameMenu;
    public CanvasGroup MainMenuCanvas;
    public CanvasGroup PauseMenuCanvas;
    public CanvasGroup RankMenuCanvas;
    public CanvasGroup SettingsMenuCanvas;
    public CanvasGroup SureMenuCanvas;
    public CanvasGroup XPSureCanvas;
    public CanvasGroup XPButtonCanvas;
    public CanvasGroup PauseButtonCanvas;
    public CanvasGroup CustomMenuCanvas;
    public CanvasGroup CheatMenuCanvas;
    public ParticleSystem previewJet;
    public Image previewShip;
    public CanvasGroup LevelEndCanvas;
    public CanvasGroup LevelInterstitialCanvas;
    public Slider warmUpSlider;
    public TextMeshProUGUI warmUpTimeText;
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI levelNameText;
    public CanvasGroup ThumbStickCanvas;
    public Slider SFXSlider;
    public TextMeshProUGUI SFXValueText;
    public Slider BGMSlider;
    public TextMeshProUGUI BGMValueText;
    public Toggle VibrationToggle;
    public Toggle PostProcessingToggle;
    public bool postProcessingEnabled;
    public AudioSource menuNoiseBox;
    public AudioClip[] menuNoises;
    public AudioMixer AudioMixer;
    public GameObject ConcentryxObject;
    public GameObject postProcessingVolume; // Reference to the Volume component
    public float minVolume = -80f; // Minimum volume in decibels
    public Coroutine warmupCoroutine;

    void Start()
    {
        previewJet.Stop();

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

        ChangePage(MainMenuCanvas, true);
        Time.timeScale = 0f;
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
        GameMaster.instance.GamePaused = false;
    }


    public void Return()
    {
        TriggerVibrate();
        ChangePage(MainMenuCanvas);
    }

    public void RankScreen()
    {
        if (GameMaster.instance.LevelEngaged)
        {
            ChangePage(RankMenuCanvas);
            Time.timeScale = 0f;
        }
    }

    public void SettingsScreen()
    {
        ChangePage(SettingsMenuCanvas);
        Time.timeScale = 0f;
    }

    public void CustomisationScreen()
    {

        ChangePage(CustomMenuCanvas);
        int shipSpriteCode = PlayerPrefs.GetInt("ShipSprite", 0);
        Ship.instance.shipSelectButton1.interactable = (shipSpriteCode == 0) ? false : true;
        Ship.instance.shipSelectButton2.interactable = (shipSpriteCode == 1) ? false : true;
        Ship.instance.shipSelectButton3.interactable = (shipSpriteCode == 2) ? false : true;
        Ship.instance.shipSelectButton4.interactable = (shipSpriteCode == 3) ? false : true;
        Ship.instance.shipSelectButton5.interactable = (shipSpriteCode == 4) ? false : true;
        Ship.instance.shipSelectButton6.interactable = (shipSpriteCode == 5) ? false : true;

        previewShip.sprite = Ship.instance.shipSpriteList[shipSpriteCode];
        previewJet.Play();


        Time.timeScale = 0f;
    }
    public void Pause()
    {
        if (GameMaster.instance.LevelEngaged)
        {
            ChangePage(PauseMenuCanvas);

            Time.timeScale = 0f;

            GameMaster.instance.GamePaused = true;
        }
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
        GameMaster.instance.ResetAll();
        ChangePage(RankMenuCanvas);

    }
    public void HardReset()
    {
        GameMaster.instance.ResetAll();
        ChangePage();
        GameMaster.instance.InstantiateLevel();

    }
    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
    public void ChangePage(CanvasGroup ToOpen = null, bool FirstRun = false)
    {
        previewJet.Stop();

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
            AudioMixer.SetFloat("SFX", -40f);
            AudioMixer.SetFloat("BGM", -40f);
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
            AudioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXVolume", 0f));
            AudioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGMVolume", 0f));
            if (!FirstRun)
            {
                menuNoiseBox.clip = menuNoises[1];
                menuNoiseBox.PlayOneShot(menuNoises[1]);
            }
        }
        TriggerVibrate();
    }
    public void TryAgain()
    {
        GameMaster.instance.ResetAll();
        TriggerVibrate();
        ChangePage(null, true);
        Time.timeScale = 1f;
        GameMaster.instance.InstantiateLevel();
    }
    public void TriggerVibrate()
    {
        Vibration.Vibrate(50, 255);
    }
    public void WarmUpGame()
    {
        warmupCoroutine = StartCoroutine(WarmUp(4));
    }
    public IEnumerator WarmUp(int timeleft)
    {
        TriggerVibrate();
        ChangePage(LevelInterstitialCanvas, true);

        GameMaster.instance.InstantiateLevel();


        levelNumberText.text = "Level " + GameMaster.instance.CurrentLevel.ToString();

        levelNameText.text = ConcentryxObject.GetComponent<Concentryx>().currentLevelName;


        while (timeleft > 0)
        {
            timeleft--;

            warmUpSlider.value = timeleft;
            warmUpTimeText.text = "Starting in " + timeleft.ToString() + "...";

            yield return new WaitForSecondsRealtime(1f);
        }


        Time.timeScale = 1f;
        ChangePage(null, true);

        GameMaster.instance.LevelEngaged = true;
    }
    public void BypassWarmup()
    {
        StopCoroutine(warmupCoroutine);
        warmupCoroutine = null;
        Time.timeScale = 1f;
        ChangePage(null, true);

        GameMaster.instance.LevelEngaged = true;
    }
}