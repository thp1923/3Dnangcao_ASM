using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DisplaySettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown, graphicsDropdown;
    public Toggle fullscreenToggle;

    List<Resolution> availableResolutions = new List<Resolution>();
    // Start is called before the first frame update
    void Start()
    {
        PopulateResolution();
        PopulateGraphics();

        fullscreenToggle.isOn = Screen.fullScreenMode != FullScreenMode.Windowed;
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    #region ResolutionFunctions/Tính năng độ phân giải
    void PopulateResolution()
    {
        resolutionDropdown.ClearOptions();
        availableResolutions.Clear();

        List<string> options = new List<string>();
        Resolution[] allRes = Screen.resolutions;
        int currentIndex = 0;
        foreach (var res in allRes)
        {
            string label = res.width + " x " + res.height;
            if (!availableResolutions.Exists(r => r.width == res.width && r.height == res.height))
            {
                availableResolutions.Add(res);
                options.Add(label);
            }
            if (res.width == Screen.width && res.height == Screen.height)
            {
                currentIndex = availableResolutions.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }
    void SetResolution(int index)
    {
        var res = availableResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }
    #endregion

    #region GraphicsFunctions/Tính năng đồ họa
    void PopulateGraphics()
    {
        graphicsDropdown.ClearOptions();
        graphicsDropdown.AddOptions(new List<string> { "Very Low", "Low", "Medium", "High" });
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();
        graphicsDropdown.onValueChanged.AddListener(QualitySettings.SetQualityLevel);
    }
    #endregion

    void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
