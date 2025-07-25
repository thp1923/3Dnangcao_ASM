using GameJolt.API;
using GameJolt.UI.Controllers;
using TMPro;
using UnityEngine;
public class AchievementList : MonoBehaviour
{
    [Header("Game Jolt Trophies Window")]
    public TrophiesWindow trophiesWindow;

    [Header("Error Display Panel")]
    public GameObject errorPanel;      
    public TMP_Text errorPanelText;    

    public void ShowAchievements()
    {
        if (!GameJoltAPI.Instance.HasSignedInUser)
        {
            ShowErrorPanel("Unable to connect to Game Jolt. Please check your internet connection.\nIf this message keeps appearing, please reinstall the game or wait for a new update. Sorry for the inconvenience.");
            return;
        }

        if (trophiesWindow != null)
        {
            trophiesWindow.gameObject.SetActive(true);
            trophiesWindow.Show(success =>
            {
                if (!success)
                {
                    ShowErrorPanel("User has not logged in Gamejolt account.");
                }
            });
        }
    }

    private void ShowErrorPanel(string message)
    {
        if (errorPanelText != null)
            errorPanelText.text = message;
        if (errorPanel != null)
            errorPanel.SetActive(true);

        CancelInvoke(nameof(HideErrorPanel));
        Invoke(nameof(HideErrorPanel), 6f);
    }

    private void HideErrorPanel()
    {
        if (errorPanel != null)
            errorPanel.SetActive(false);
    }
}
