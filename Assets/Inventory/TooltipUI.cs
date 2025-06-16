using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI valueText;

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
        typeText.text = $"Type: {item.itemType} | Rarity: {item.rarity}";
        valueText.text = $"Value: {item.value}";
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
