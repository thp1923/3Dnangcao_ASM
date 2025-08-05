using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestCardUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionDescriptionText;
    public TextMeshProUGUI missionTypeText;
    public Image typeBackgroundImage; // Panel chứa chữ “Chính” hoặc “Phụ”

    [Header("Color Settings")]
    public Color mainColor = new Color(1f, 0.92f, 0.016f); // Vàng (Chính)
    public Color sideColor = Color.gray;                  // Xám (Phụ)

    public void Setup(Mission mission)
    {
        if (mission == null || missionTypeText == null || typeBackgroundImage == null)
        {
            Debug.LogWarning("Missing references in QuestCardUI");
            return;
        }

        // Gán dữ liệu text
        missionNameText.text = mission.missionName;
        missionDescriptionText.text = mission.missionDescription;
        missionTypeText.text = mission.isMainStory ? "Main" : "Side";

        // Đổi màu panel
        typeBackgroundImage.color = mission.isMainStory ? mainColor : sideColor;
    }
}
