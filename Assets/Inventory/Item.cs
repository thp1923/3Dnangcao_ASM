using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.EconomyModels;
using UnityEngine;

public enum ItemType { Consumable, Equipment, Quest, Material }
public enum Rarity { Common, Uncommon, Rare, Epic, Legendary, Mythical }

[CreateAssetMenu(menuName = "InventoryThaiAnh/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public Rarity rarity;
    public int maxStack;
    public int value; // giá tiền
    public GameObject prefab;
}
