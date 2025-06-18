using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSaveData
{
    public int id;
    public int amount;
}

[System.Serializable]
public class InventorySaveWrapper
{
    public List<ItemSaveData> inventory = new List<ItemSaveData>();
}

public class InventoryPlayfabHandler : MonoBehaviour
{
    public void SaveInventory(List<InventorySlot> slots)
    {
        var data = new InventorySaveWrapper();

        foreach (var slot in slots)
        {
            if (slot.IsEmpty()) continue;

            var item = slot.GetItem();
            int count = slot.GetStackCount();

            data.inventory.Add(new ItemSaveData { id = item.ItemID, amount = count });
        }

        string json = JsonUtility.ToJson(data);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { "InventoryData", json } }
        };

        PlayFabClientAPI.UpdateUserData(request,
            result => Debug.Log("✅ Inventory saved!"),
            error => Debug.LogError("❌ Save failed: " + error.GenerateErrorReport()));
    }

    public void LoadInventory(System.Action<List<ItemSaveData>> onLoaded)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("InventoryData"))
            {
                var json = result.Data["InventoryData"].Value;
                var data = JsonUtility.FromJson<InventorySaveWrapper>(json);
                onLoaded?.Invoke(data.inventory);
            }
            else
            {
                Debug.Log("📭 No inventory data found.");
                onLoaded?.Invoke(new List<ItemSaveData>());
            }
        },
        error => Debug.LogError("❌ Load failed: " + error.GenerateErrorReport()));
    }
}
