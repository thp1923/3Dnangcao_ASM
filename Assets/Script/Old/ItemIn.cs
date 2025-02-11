using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemIn", menuName = "Inventory/Item")]
public class ItemIn : ScriptableObject
{

    public enum ItemType
    {
        Consumbles, Weapon
    }
    public int Id;
    public string Item_Name;
    public int value;
    public Sprite image;
    public ItemType itemType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
