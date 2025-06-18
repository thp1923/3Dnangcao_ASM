using UnityEngine;
using UnityEngine.UI;

public class ScrollPositionKeeper : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    // Lưu giá trị normalized position trước khi update nội dung
    private float savedPos;

    public void BeforeUpdateContent()
    {
        savedPos = scrollRect.verticalNormalizedPosition;
    }

    public void AfterUpdateContent()
    {
        // Cần force update layout trước khi gán lại
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = savedPos;
    }
}
