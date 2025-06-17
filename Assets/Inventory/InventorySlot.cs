using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class InventorySlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IBeginDragHandler,
    IEndDragHandler, IDropHandler,
    IDragHandler, IPointerClickHandler
{
    public Image icon;
    private Item currentItem;
    public EquidmentSlotType slotType;
    public TextMeshProUGUI stackText;
    public bool isEquipmentSlot = false;
    private int stackCount = 0;
    private float lastClickTime;
    private float doubleClickThreshold = 0.3f;

    public void AddItem(Item newItem, int amount = 1)
    {
        // Nếu là Equipment slot hoặc item không stack → luôn gán mới
        if (isEquipmentSlot || newItem.itemType == ItemType.Equipment ||
            newItem.itemType == ItemType.Weapon || newItem.itemType == ItemType.Armor)
        {
            currentItem = newItem;
            stackCount = 1;
            icon.sprite = newItem.icon;
            icon.enabled = true;
            UpdateStackText();
            return;
        }

        // Nếu có thể stack (đúng ID + usable giống nhau)
        if (currentItem != null && newItem.ItemID == currentItem.ItemID &&
            newItem.isUsable == currentItem.isUsable)
        {
            stackCount += amount;
            UpdateStackText();
            return;
        }

        // Nếu là item mới
        currentItem = newItem;
        stackCount = amount;
        icon.sprite = newItem.icon;
        icon.enabled = true;
        UpdateStackText();
    }

    public void RemoveOneItem()
    {
        if (stackCount > 1)
        {
            stackCount--;
            UpdateStackText();
        }
        else
        {
            ClearSlot();
        }
    }

    public bool CanStack(Item item)
    {
        if (isEquipmentSlot || item.itemType == ItemType.Equipment ||
         item.itemType == ItemType.Weapon || item.itemType == ItemType.Armor)
            return false;

        return currentItem != null &&
               item.ItemID == currentItem.ItemID &&
               stackCount < currentItem.maxStack;
    }

    private void UpdateStackText()
    {
        if (stackText == null) return; // Không update nếu là Equipment
        if (isEquipmentSlot || stackText == null)
        {
            stackText.text = "";
            return;
        }
        stackText.text = stackCount > 1 ? stackCount.ToString() : "";
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
        //Debug.Log("[OnPointerDown] Click vào slot");
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
        DragItem.Instance?.Hide();
    }

    public void OnDrop(PointerEventData eventData)
{
    InventorySlot draggedFrom = eventData.pointerDrag?.GetComponent<InventorySlot>();
    if (draggedFrom == null || draggedFrom == this) return;

    Item draggedItem = draggedFrom.GetItem();
    if (draggedItem == null) return;

    // Nếu thả vào Equipment slot
    if (isEquipmentSlot)
    {
        if (!IsEquipment(draggedItem.itemType) || slotType != draggedItem.allowedSlot)
        {
            Debug.LogWarning("[OnDrop] Slot không hợp lệ");
            return;
        }
    }

    // Nếu kéo từ Equipment → Bag
    if (draggedFrom.isEquipmentSlot && !isEquipmentSlot)
    {
        InventoryManager.Instance.UnequipItem(draggedFrom);
        DragItem.Instance.Hide();
        return;
    }

    // ✅ Nếu gộp được
    if (!isEquipmentSlot && CanStack(draggedItem))
    {
        int spaceLeft = currentItem.maxStack - stackCount;
        int fromCount = draggedFrom.GetStackCount();

        if (fromCount <= spaceLeft)
        {
            stackCount += fromCount;
            UpdateStackText();
            draggedFrom.ClearSlot(true);
        }
        else
        {
            stackCount = currentItem.maxStack;
            draggedFrom.UpdateStackCount(fromCount - spaceLeft);
            UpdateStackText();
        }

        DragItem.Instance.Hide();
        return; // 🔒 PHẢI return ở đây nếu đã gộp để KHÔNG swap nữa
    }

    // ❌ Nếu không stack được → hoán đổi
    Item tempItem = currentItem;
    int tempCount = stackCount;

    currentItem = null; // tránh gộp chồng
    stackCount = 0;

    AddItem(draggedItem, draggedFrom.GetStackCount());

    if (tempItem == null)
    {
        draggedFrom.ClearSlot(true);
    }
    else
    {
        draggedFrom.ClearSlot();
        draggedFrom.AddItem(tempItem, tempCount);
    }

    DragItem.Instance.Hide();
}



    public int GetStackCount()
    {
        return stackCount;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && currentItem != null)
        {
            if (isEquipmentSlot)
            {
                InventoryManager.Instance.CreateBagSlot(currentItem);
                ClearSlot();
                return;
            }

            if (!IsEquipment(currentItem.itemType))
            {
                Debug.LogWarning("[Ctrl+Click] Vật phẩm không hợp lệ để trang bị: " + currentItem.itemName);
                return;
            }

            InventoryManager.Instance.TryMoveItem(this);
            return;
        }

        // Nếu click 2 lần nhanh → dùng item
        if (currentItem != null && currentItem.isUsable)
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            lastClickTime = Time.time;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                currentItem.Use(); // Gọi logic xử lý item

                if (stackCount <= 1)
                    ClearSlot(true); // Huỷ slot khi stack = 1
                else
                    RemoveOneItem(); // Giảm 1 nếu còn nhiều
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Để Unity xử lý OnDrag nếu cần.
    }

    public Item GetItem()
    {
        return currentItem;
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    private bool IsEquipment(ItemType type)
    {
        return type == ItemType.Equipment || type == ItemType.Weapon || type == ItemType.Armor;
    }
    public void ClearSlot(bool destroySlot = false)
    {
        Debug.Log("[ClearSlot] Xoá item: " + (currentItem != null ? currentItem.itemName : "null"));
        currentItem = null;
        stackCount = 0;
        icon.sprite = null;
        icon.enabled = false;
        UpdateStackText();

        if (destroySlot && !isEquipmentSlot)
        {
            Destroy(gameObject); // Huỷ slot nếu là bag slot
            Debug.Log("[ClearSlot] Đã huỷ slot khỏi Bag");
        }
    }
    public void UpdateStackCount(int amount)
{
    stackCount = amount;
    UpdateStackText();

    if (stackCount <= 0)
        ClearSlot(true);
}

}
