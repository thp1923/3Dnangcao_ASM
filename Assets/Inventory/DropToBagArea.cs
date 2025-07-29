using UnityEngine;
using UnityEngine.EventSystems;

public class DropToBagArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();
        if (draggedSlot == null || !draggedSlot.isEquipmentSlot) return;

        Item draggedItem = draggedSlot.GetItem();
        if (draggedItem == null) return;

        // Tạo lại slot trong Bag
        InventoryManager.Instance.UnequipItem(draggedSlot);
        //DragItem.Instance.Hide();

        Debug.Log($"[DropToBagArea] Tạo slot mới cho: {draggedItem.itemName}");
    }
}