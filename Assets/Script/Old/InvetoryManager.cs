using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class InvetoryManager : MonoBehaviour
{

    public static InvetoryManager Instance { get; private set; }
    public List<ItemIn> items = new List<ItemIn>();
    public Transform itemHolder;
    public GameObject itemPrefab;
    public Toggle enableRomoveButton;
    public void DislayInventory()
    {
        foreach(Transform item in itemHolder)
        {
            Destroy(item.gameObject);
        }
        foreach(ItemIn item in items)
        {
            GameObject obj = Instantiate(itemPrefab, itemHolder);
            TextMeshProUGUI itemName = obj.transform.Find("ItemName")
                .GetComponent<TextMeshProUGUI>();
            Image itemImage = obj.transform.Find("ItemImage")
                .GetComponent<Image>();
            itemName.text = item.Item_Name;
            itemImage.sprite = item.image;
            obj.GetComponent<ItemUIController>().SetItem(item);
        }
        EnableRemoveButton();
        
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void Add(ItemIn item)
    {
        items.Add(item);
        DislayInventory();
    }

    public void Remove(ItemIn item)
    {
        items.Remove(item);
    }
    
    public void EnableRemoveButton()
    {
        if (enableRomoveButton.isOn)
        {
            foreach(Transform item in itemHolder)
                item.transform.Find("RemoveButton")
                    .gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform item in itemHolder)
                item.transform.Find("RemoveButton")
                    .gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
