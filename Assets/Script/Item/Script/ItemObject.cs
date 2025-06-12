using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum itemtype
{
    Heal,
    Equidment,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public itemtype type;
    [TextArea(15,20)]
    public string description;
}
