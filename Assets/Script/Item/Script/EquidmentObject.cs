using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equidment Object",menuName ="Inventory System/Item/Equidment")]
public class EquidmentObject : ItemObject
{
    public float atkBonus;
    public float defenceBonus;
    public void Awake()
    {
        type = itemtype.Equidment;
    }
}
