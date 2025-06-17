using UnityEngine;
using UnityEngine.UI;

public class DragItem : MonoBehaviour
{
    public static DragItem Instance;

    public Image icon;
    private RectTransform rectTransform;

    void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent as RectTransform,
                Input.mousePosition, null, out pos);
            rectTransform.localPosition = pos;
        }
    }

    public void Show(Sprite sprite)
{
    if (sprite == null)
    {
        //Debug.LogError("[DragItem] Sprite truyền vào bị null!");
        return;
    }

    icon.sprite = sprite;
    icon.enabled = true;
    gameObject.SetActive(true);
    //Debug.Log("[DragItem] Hiện icon kéo");
}


    public void Hide()
    {
        gameObject.SetActive(false);
        //Debug.Log("[DragItem] Ẩn icon kéo");
    }
}
