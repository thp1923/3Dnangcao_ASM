using GameJolt.API;
using GameJolt.UI.Controllers;
using TMPro;
using UnityEngine;
public class AchievementList : MonoBehaviour
{
    [Header("Game Jolt Trophies Window")]
    public TrophiesWindow trophiesWindow;

    [Header("Error Display")]
    public TMP_Text errorText;
    public void ShowAchievements()
    {
        if (!GameJoltAPI.Instance.HasSignedInUser)
        {
            ShowError("Unable to connect to Game Jolt. Please check your internet connection.\nIf this message keeps appearing, please reinstall the game or wait for a new update. Sorry for the inconvenience.");
            return;
        }

        if (trophiesWindow != null)
        {
            trophiesWindow.gameObject.SetActive(true);
            trophiesWindow.Show(success =>
            {
                if (!success)
                {
                    ShowError("Failed to load achievements.");
                }
            });
        }
        else
        {
            ShowError("TrophiesWindow is not assigned.");
        }
    }

    private void ShowError(string message)
    {
        if (errorText == null) return;

        errorText.text = message;
        errorText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideError));
        Invoke(nameof(HideError), 6f);
    }

    private void HideError()
    {
        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }
}
