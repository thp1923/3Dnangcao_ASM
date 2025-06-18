using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    private static List<Item> allItems;

    // Load một lần duy nhất
    public static void LoadItems()
    {
        if (allItems != null) return; // đã load rồi

        allItems = new List<Item>(Resources.LoadAll<Item>("Items"));

        Debug.Log($"[ItemDatabase] Loaded {allItems.Count} items from Resources/Items/");
    }

    public static Item GetItemByID(int id)
    {
        LoadItems(); // Đảm bảo đã load
        return allItems.Find(item => item.ItemID == id);
    }

    public static List<Item> GetAllItems()
    {
        LoadItems();
        return allItems;
    }
}
