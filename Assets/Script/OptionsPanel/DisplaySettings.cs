using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class DisplaySettings : MonoBehaviour
{
    #region UI Fields
    [Header("UI Components")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicsDropdown;
    public Toggle fullscreenToggle;
    public Slider brightnessSlider;
    public Slider gammaSlider;
    #endregion

    #region Volume Fields
    [Header("Volume")]
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;
    private LiftGammaGain liftGammaGain;
    #endregion

    private List<Resolution> availableResolutions = new List<Resolution>();

    void Start()
    {
        SetupVolumeReferences();
        SetupResolutionOptions();
        SetupGraphicsOptions();
        SetupFullscreenToggle();
        SetupBrightnessSlider();
        SetupGammaSlider();
        LoadSettings();
    }

    #region Setup

    void SetupVolumeReferences()
    {
        if (globalVolume == null)
        {
            Debug.LogWarning("Global Volume not assigned in DisplaySettings.");
            return;
        }
        globalVolume.profile.TryGet(out colorAdjustments);
        globalVolume.profile.TryGet(out liftGammaGain);
    }

    void SetupResolutionOptions()
    {
        resolutionDropdown.ClearOptions();
        availableResolutions.Clear();

        List<string> options = new List<string>();
        Resolution[] allResolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            Resolution res = allResolutions[i];
            string label = $"{res.width} x {res.height}";
            if (!availableResolutions.Exists(r => r.width == res.width && r.height == res.height))
            {
                availableResolutions.Add(res);
                options.Add(label);
            }
            if (res.width == Screen.width && res.height == Screen.height)
            {
                currentResolutionIndex = availableResolutions.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener((index) => { SetResolution(index); SaveSettings(); });
    }

    void SetupGraphicsOptions()
    {
        graphicsDropdown.ClearOptions();
        graphicsDropdown.AddOptions(new List<string> { "Very Low", "Low", "Medium", "High" });
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();
        graphicsDropdown.onValueChanged.AddListener((level) => { SetGraphicsQuality(level); SaveSettings(); });
    }

    void SetupFullscreenToggle()
    {
        fullscreenToggle.isOn = Screen.fullScreenMode != FullScreenMode.Windowed;
        fullscreenToggle.onValueChanged.AddListener((isFullscreen) => { SetFullScreen(isFullscreen); SaveSettings(); });
    }

    void SetupBrightnessSlider()
    {
        brightnessSlider.minValue = -2f;
        brightnessSlider.maxValue = 2f;
        brightnessSlider.value = colorAdjustments != null ? colorAdjustments.postExposure.value : 0f;
        brightnessSlider.onValueChanged.AddListener((value) => { SetBrightness(value); SaveSettings(); });
    }

    void SetupGammaSlider()
    {
        gammaSlider.minValue = 0.5f;
        gammaSlider.maxValue = 2f;
        gammaSlider.value = liftGammaGain != null ? liftGammaGain.gamma.value.x : 1f;
        gammaSlider.onValueChanged.AddListener((value) => { SetGamma(value); SaveSettings(); });
    }

    #endregion

    #region UI Event Handlers

    void SetResolution(int index)
    {
        if (index < 0 || index >= availableResolutions.Count) return;
        Resolution selectedRes = availableResolutions[index];
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreenMode);
    }

    void SetGraphicsQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    void SetBrightness(float value)
    {
        if (colorAdjustments != null)
            colorAdjustments.postExposure.value = value;
    }

    void SetGamma(float value)
    {
        if (liftGammaGain != null)
            liftGammaGain.gamma.value = new Vector4(value, value, value, 0f);
    }

    #endregion

    #region Save/Load Settings

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("GraphicsQuality", graphicsDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        PlayerPrefs.SetFloat("Gamma", gammaSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        // Only load if saved before
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            int resIndex = PlayerPrefs.GetInt("ResolutionIndex");
            if (resIndex >= 0 && resIndex < availableResolutions.Count)
            {
                resolutionDropdown.value = resIndex;
                SetResolution(resIndex);
            }
        }

        if (PlayerPrefs.HasKey("GraphicsQuality"))
        {
            int quality = PlayerPrefs.GetInt("GraphicsQuality");
            graphicsDropdown.value = quality;
            SetGraphicsQuality(quality);
        }

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
            fullscreenToggle.isOn = isFullscreen;
            SetFullScreen(isFullscreen);
        }

        if (PlayerPrefs.HasKey("Brightness"))
        {
            float brightness = PlayerPrefs.GetFloat("Brightness");
            brightnessSlider.value = brightness;
            SetBrightness(brightness);
        }

        if (PlayerPrefs.HasKey("Gamma"))
        {
            float gamma = PlayerPrefs.GetFloat("Gamma");
            gammaSlider.value = gamma;
            SetGamma(gamma);
        }
    }

    #endregion
}
