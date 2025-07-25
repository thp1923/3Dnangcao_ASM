using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class InventoryManager : MonoBehaviour
{
    public GameObject bagSlotPrefab;  // Dùng để tạo item trong túi
    public GameObject equipmentSlotPrefab; // Dùng để tạo giao diện Equipment nếu cần

    public List<Item> testItems; // Gán item từ Inspector
    public GameObject itemSlotPrefab; // Gán prefab
    public Transform contentPanel; // ScrollView > Content
    public List<InventorySlot> equipmentSlots = new List<InventorySlot>();

    public static InventoryManager Instance { get; private set; }


    void Start()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {

            //LoadInventory();

        }
        foreach (Item item in testItems)
        {
            GameObject slotGO = Instantiate(itemSlotPrefab, contentPanel);
            InventorySlot slot = slotGO.GetComponentInChildren<InventorySlot>(); // hoặc GetComponent nếu script ở gốc
            slot.AddItem(item);
        }

        foreach (var slot in equipmentSlots)
        {
            if (slot.IsEmpty())
            {
                slot.icon.enabled = false;
            }
        }
        MergeAllStackableItems();
        //SaveInventory();

    }
    public void TryMoveItem(InventorySlot fromSlot)
    {
        Item item = fromSlot.GetItem();
        if (item == null) return;
        if (!IsEquipment(item.itemType)) return;

        foreach (var slot in equipmentSlots)
        {
            if (!slot.isEquipmentSlot || slot.slotType != item.allowedSlot)
                continue;

            if (!slot.IsEmpty())
            {
                Item tempItem = slot.GetItem();
                int tempCount = slot.GetStackCount();

                slot.AddItem(item, fromSlot.GetStackCount());
                fromSlot.AddItem(tempItem, tempCount);
            }
            else
            {
                slot.AddItem(item, fromSlot.GetStackCount());

                Debug.Log($"[TryMoveItem] fromSlot: {fromSlot.name}, isEquipmentSlot: {fromSlot.isEquipmentSlot}");

                // ✅ Đây là điểm cần chắc chắn destroy slot nếu từ túi
                if (!fromSlot.isEquipmentSlot)
                {
                    fromSlot.ClearSlot(true); // Destroy gameObject
                    Debug.Log("[Destroy] Đã huỷ slot túi sau khi trang bị");
                }
                else
                {
                    fromSlot.ClearSlot(); // Equipment slot: chỉ clear item
                }
            }

            //SaveInventory();

            return;
        }
    }


    public void CreateBagSlot(Item item, int amount = 1)
    {
        GameObject slotGO = Instantiate(bagSlotPrefab, contentPanel);
        InventorySlot newSlot = slotGO.GetComponent<InventorySlot>();
        newSlot.AddItem(item, amount);

        //SaveInventory();

    }


    bool IsEquipment(ItemType type)
    {
        return type == ItemType.Equipment;
    }
    void Awake()
    {
        Instance = this;
    }
    public void UnequipItem(InventorySlot fromSlot)
    {
        Item item = fromSlot.GetItem();
        if (item == null) return;

        GameObject newSlotGO = Instantiate(itemSlotPrefab, contentPanel);
        InventorySlot newSlot = newSlotGO.GetComponentInChildren<InventorySlot>();
        newSlot.AddItem(item);

        fromSlot.ClearSlot();
        Debug.Log($"[Unequip] {item.itemName} → tạo lại trong Bag");

        //SaveInventory();

    }

    public void TryAddToInventory(Item item)
    {
        // Tìm slot có thể stack
        foreach (Transform child in contentPanel)
        {
            var slot = child.GetComponentInChildren<InventorySlot>();
            if (slot != null && slot.CanStack(item))
            {
                slot.AddItem(item, 1);
                MergeAllStackableItems();

                //SaveInventory();

                return;
            }
        }

        // Nếu không có slot stack được -> tạo mới
        GameObject go = Instantiate(itemSlotPrefab, contentPanel);
        InventorySlot newSlot = go.GetComponentInChildren<InventorySlot>();
        newSlot.AddItem(item);

        MergeAllStackableItems();

        //SaveInventory();

    }

    public void MergeAllStackableItems()
    {
        Dictionary<int, InventorySlot> mergedSlots = new Dictionary<int, InventorySlot>();

        foreach (Transform child in contentPanel)
        {
            InventorySlot slot = child.GetComponentInChildren<InventorySlot>();
            if (slot == null || slot.IsEmpty()) continue;

            Item item = slot.GetItem();
            int id = item.ItemID;

            if (!mergedSlots.ContainsKey(id))
            {
                mergedSlots[id] = slot;
            }
            else
            {
                InventorySlot target = mergedSlots[id];
                if (target.CanStack(item))
                {
                    int available = item.maxStack - target.GetStackCount();
                    int moving = Mathf.Min(available, slot.GetStackCount());

                    target.AddItem(item, moving);
                    slot.UpdateStackCount(slot.GetStackCount() - moving);
                }
            }
        }

        //SaveInventory();
    }
    /*

    /////////////////////////////////////Lưu Trên PlayFab///////////////////////////////////
    public void SaveInventory()
    {
        List<InventoryItemData> bagItems = new List<InventoryItemData>();
        List<InventoryItemData> equipItems = new List<InventoryItemData>();

        // Lưu túi
        foreach (Transform child in contentPanel)
        {
            InventorySlot slot = child.GetComponentInChildren<InventorySlot>();
            if (slot != null && !slot.IsEmpty())
            {
                bagItems.Add(new InventoryItemData
                {
                    itemId = slot.GetItem().ItemID,
                    quantity = slot.GetStackCount(),
                    slotType = "Bag"
                });
            }
        }

        // Lưu trang bị
        foreach (var slot in equipmentSlots)
        {
            if (!slot.IsEmpty())
            {
                equipItems.Add(new InventoryItemData
                {
                    itemId = slot.GetItem().ItemID,
                    quantity = 1,
                    slotType = slot.slotType.ToString()
                });
            }
        }

        // Gộp thành dictionary
        var data = new Dictionary<string, string>
    {
        { "Bag", JsonUtility.ToJson(new InventoryWrapper { items = bagItems }) },
        { "Equip", JsonUtility.ToJson(new InventoryWrapper { items = equipItems }) }
    };

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = data
        },
        result => Debug.Log("[SaveInventory] ✅ Thành công"),
        error => Debug.LogError("[SaveInventory] ❌ Thất bại: " + error.ErrorMessage));
    }

    [System.Serializable]
    public class InventoryWrapper
    {
        public List<InventoryItemData> items;
    }

    //////////////////////////////Load PlayFab ///////////////////////////////////////
    public void LoadInventory()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null)
            {
                // Dọn sạch túi và trang bị hiện tại
                foreach (Transform child in contentPanel)
                    Destroy(child.gameObject);
                foreach (var slot in equipmentSlots)
                    slot.ClearSlot();

                // Load túi
                if (result.Data.ContainsKey("Bag"))
                {
                    InventoryWrapper bagData = JsonUtility.FromJson<InventoryWrapper>(result.Data["Bag"].Value);
                    foreach (var itemData in bagData.items)
                    {
                        Item item = ItemDatabase.GetItemByID(itemData.itemId); // Bạn cần hàm này
                        if (item != null)
                            CreateBagSlot(item, itemData.quantity);
                    }
                }

                // Load trang bị
                if (result.Data.ContainsKey("Equip"))
                {
                    InventoryWrapper equipData = JsonUtility.FromJson<InventoryWrapper>(result.Data["Equip"].Value);
                    foreach (var itemData in equipData.items)
                    {
                        Item item = ItemDatabase.GetItemByID(itemData.itemId);
                        if (item != null)
                        {
                            foreach (var slot in equipmentSlots)
                            {
                                if (slot.slotType.ToString() == itemData.slotType)
                                {
                                    slot.AddItem(item);
                                    break;
                                }
                            }
                        }
                    }
                }

                Debug.Log("[LoadInventory] ✅ Tải dữ liệu thành công");
            }
            else
            {
                Debug.LogWarning("[LoadInventory] ⚠ Không có dữ liệu");
            }
        },
        error =>
        {
            Debug.LogError("[LoadInventory] ❌ Thất bại: " + error.ErrorMessage);
        });

    }*/

}
