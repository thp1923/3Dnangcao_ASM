using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.EconomyModels;
using UnityEngine;

public enum ItemType { Consumable, Equipment, Quest, Material, Weapon,Armor}
public enum Rarity { Common, Uncommon, Rare, Epic, Legendary, Mythical }
public enum EquidmentSlotType {Head, Chest, Leg, Weapon, Shield, Ring}

[CreateAssetMenu(menuName = "InventoryThaiAnh/Item")]
public class Item : ScriptableObject
{
    public int ItemID;
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public Rarity rarity;
    public int maxStack = 1;
    public int value; // giá tiền
    public EquidmentSlotType allowedSlot;
    public GameObject prefab;
    public bool isUsable;

    [Header("3D Model Prefab")]
    public GameObject modelPrefab;  // prefab để spawn mô hình vũ khí/giáp

    public virtual void Use()
    {
        Debug.Log("Used " + itemName);
        // Nếu là HP Potion: hồi máu
        // Nếu là Buff: tăng tốc
        // Nếu là Scroll: mở cửa,...
    }
}
[CreateAssetMenu(menuName = "InventoryThaiAnh/HealthPotion")]
public class HealthPotion : Item
{
    public int healAmount;

    public override void Use()
    {
        base.Use();
        Debug.Log("Healed for " + healAmount + " HP");
        // Gọi PlayerHealth.Instance.Heal(healAmount) chẳng hạn
    }
}


