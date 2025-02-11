using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class InvetoryManager : MonoBehaviour
{

    public static InvetoryManager Instance { get; private set; }
    public List<ItemIn> items = new List<ItemIn>();
    //public List<ItemModel> itemsMl = new List<ItemModel>();
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

    public void Add(ItemIn item/*, ItemModel itemMl*/)
    {
        items.Add(item);
        //itemsMl.Add(itemMl);
        DislayInventory();
    }

    //public void DropItem(ItemModel itemMl)
    //{
    //    Transform transPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    //    GameObject instance = Instantiate(itemMl.itemModel, 
    //        transPlayer.position
    //        + new Vector3(Random.Range(-0.7f, 0.7f), 0, Random.Range(-0.7f, 0.7f)), Quaternion.identity);
    //    itemsMl.Remove(itemMl);
    //}
    public void Remove(ItemIn item/*, ItemModel itemMl*/)
    {
        items.Remove(item);
        //DropItem(itemMl);
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
