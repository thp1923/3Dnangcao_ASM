using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsPanel : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicsDropdown;
    private List<Resolution> availableResolutions = new List<Resolution>();

    void Start()
    {
        PopulateResolutionDropdown();
        PopulateGraphicsDropdown();
    }

    #region Resolution
    void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        Resolution[] allResolutions = Screen.resolutions;
        availableResolutions.Clear();

        int currentResolutionIndex = 0;
        Resolution currentRes = Screen.currentResolution;

        foreach (Resolution res in allResolutions)
        {
            string option = res.width + " x " + res.height + " @ " + res.refreshRateRatio.numerator / res.refreshRateRatio.denominator + "Hz";

            // Prevent duplicate resolutions (same width, height, refresh rate)
            if (!availableResolutions.Exists(r => r.width == res.width && r.height == res.height && r.refreshRateRatio.numerator == res.refreshRateRatio.numerator && r.refreshRateRatio.denominator == res.refreshRateRatio.denominator))
            {
                availableResolutions.Add(res);
                options.Add(option);
            }

            // Set default index to the current resolution
            if (res.width == currentRes.width && res.height == currentRes.height && res.refreshRateRatio.numerator == currentRes.refreshRateRatio.numerator && res.refreshRateRatio.denominator == currentRes.refreshRateRatio.denominator)
            {
                currentResolutionIndex = availableResolutions.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }
    #endregion

    #region Graphics
    void PopulateGraphicsDropdown()
    {
        graphicsDropdown.ClearOptions();
        List<string> options = new List<string> { "Very Low","Low", "Medium", "High", "Ultra" };
        graphicsDropdown.AddOptions(options);
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();
        graphicsDropdown.onValueChanged.AddListener(SetGraphics);
    }
    #endregion

    #region Panel features
    #region Resolution feature
    void AdjustCameraForResolution()
    {
        float defaultAspect = 16f / 9f;
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect < defaultAspect)
        {
            Camera.main.fieldOfView = Mathf.Lerp(60, 70, (defaultAspect - currentAspect) * 2);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= availableResolutions.Count)
            return;

        Resolution resolution = availableResolutions[resolutionIndex];

        if (Screen.width != resolution.width || Screen.height != resolution.height || Screen.currentResolution.refreshRateRatio.numerator != resolution.refreshRateRatio.numerator || Screen.currentResolution.refreshRateRatio.denominator != resolution.refreshRateRatio.denominator)
        {
            // Convert refresh rate to a RefreshRate struct
            RefreshRate refreshRate = new RefreshRate { numerator = resolution.refreshRateRatio.numerator, denominator = resolution.refreshRateRatio.denominator };

            // Use FullScreenMode instead of bool
            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow, refreshRate);

            Invoke(nameof(AdjustCameraForResolution), 0.1f);
        }
    }
    #endregion

    #region Graphic feature
    public void SetGraphics(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    #endregion
    #endregion
}
