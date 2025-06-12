using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverBackground : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backgroundImage;

    void Start()
    {
        if (backgroundImage != null)
        {
            SetAlpha(0f); // ẩn khi chưa hover
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (backgroundImage != null)
            SetAlpha(1f); // hiện nền khi hover
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (backgroundImage != null)
            SetAlpha(0f); // ẩn nền khi không hover
    }

    private void SetAlpha(float a)
    {
        Color color = backgroundImage.color;
        color.a = a;
        backgroundImage.color = color;
    }
}
