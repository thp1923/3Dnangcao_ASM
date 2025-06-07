using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IBeginDragHandler,
    IEndDragHandler, IDropHandler,
    IDragHandler // ⚠️ thêm dòng này


{
    public Image icon;
    private Item currentItem;

    public void AddItem(Item newItem)
    {
        currentItem = newItem;
        if (icon != null && newItem.icon != null)
        {
            icon.sprite = newItem.icon;
            icon.enabled = true;
            Debug.Log("[AddItem] Đã thêm item: " + newItem.itemName);
        }
        else
        {
            Debug.LogWarning("[AddItem] Icon hoặc newItem.icon bị null");
        }
    }

    public void ClearSlot()
    {
        Debug.Log("[ClearSlot] Xoá item: " + (currentItem != null ? currentItem.itemName : "null"));
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            TooltipUI.Instance.ShowTooltip(currentItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideTooltip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("[OnPointerDown] Click vào slot");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("[OnBeginDrag] Click vào slot: " + gameObject.name);

        if (currentItem != null)
        {
            Debug.Log("[OnBeginDrag] Kéo item: " + currentItem.itemName);
            if (icon.sprite == null)
            {
                Debug.LogError("[OnBeginDrag] ICON.SPRITE = NULL");
            }
            else
            {
                DragItem.Instance?.Show(icon.sprite);
            }
        }
        else
        {
            Debug.LogWarning("[OnBeginDrag] currentItem = null!");
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("[OnEndDrag] Thả kéo");
        if (DragItem.Instance != null)
        {
            DragItem.Instance.Hide();
        }
        else
        {
            Debug.LogError("[OnEndDrag] DragItem.Instance = null!");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("[OnDrop] Có thả vật vào đây");
        InventorySlot draggedFrom = eventData.pointerDrag?.GetComponent<InventorySlot>();

        if (draggedFrom == null)
        {
            Debug.LogWarning("[OnDrop] draggedFrom = null");
            return;
        }
        if (draggedFrom == this)
        {
            Debug.Log("[OnDrop] draggedFrom chính là slot hiện tại (bỏ qua)");
            return;
        }

        Debug.Log("[OnDrop] Hoán đổi item: " + draggedFrom.currentItem?.itemName + " ↔ " + currentItem?.itemName);

        Item tempItem = currentItem;
        AddItem(draggedFrom.currentItem);

        if (tempItem == null)
        {
            draggedFrom.ClearSlot();
        }
        else
        {
            draggedFrom.AddItem(tempItem);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Bắt buộc để Unity nhận diện bạn đang kéo
        // Có thể để trống, hoặc thêm log nếu cần:
        // Debug.Log("[OnDrag] Đang kéo...");
    }

}
