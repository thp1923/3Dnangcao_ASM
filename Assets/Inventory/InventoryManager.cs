using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> testItems; // Gán item từ Inspector
    public GameObject itemSlotPrefab; // Gán prefab
    public Transform contentPanel; // ScrollView > Content

    void Start()
{
    foreach (Item item in testItems)
    {
        GameObject slotGO = Instantiate(itemSlotPrefab, contentPanel);
        InventorySlot slot = slotGO.GetComponentInChildren<InventorySlot>(); // hoặc GetComponent nếu script ở gốc
        slot.AddItem(item);
    }
}


}
