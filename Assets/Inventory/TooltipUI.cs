using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rarityText;

    void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    void Update()
    {
    }

    public void ShowTooltip(Item item)
    {
        tooltipPanel.SetActive(true);
        nameText.text = item.itemName;
        descriptionText.text = $"{item.description}";
        rarityText.text = $"Rarity: {item.rarity}";
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
