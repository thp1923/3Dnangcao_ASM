using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Heal Object",menuName ="Inventory System/Item/Heal")]
public class HealObject : ItemObject
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = itemtype.Heal;
    }
}
