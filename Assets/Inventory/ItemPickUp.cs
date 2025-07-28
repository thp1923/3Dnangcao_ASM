using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    private bool isPickedUp = false;

    [Header("Prefab UI hiển thị item")]
    public GameObject UI;

    private Transform viewportContent;

    [Header("Danh sách item được nhặt")]
    public List<Item> items;

    InventoryManager inventoryManager;

    void Start()
    {
        // Tìm Content (ScrollView/Viewport/Content) gán tag "ItemView"
        inventoryManager = InventoryManager.Instance;
        GameObject contentObj = GameObject.FindWithTag("ItemView");
        if (contentObj != null)
            viewportContent = contentObj.transform;
        else
            Debug.LogError("[ItemPickUp] ❌ Không tìm thấy GameObject có tag 'ItemView'");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPickedUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPickedUp = false;
        }
    }

    void Update()
    {
        if (isPickedUp && Input.GetKeyDown(KeyCode.F))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        foreach (var item in items)
        {
            GameObject go = Instantiate(UI, viewportContent); // Tạo UI mới vào content

            // Gán icon
            Image icon = go.GetComponentInChildren<Image>();
            if (icon != null)
                icon.sprite = item.icon;

            // Gán tên item
            TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = item.itemName;
            if(item != null)
                inventoryManager.TryAddToInventory(item);
        }

        // Xoá object sau khi nhặt
        Destroy(gameObject);
    }
}
