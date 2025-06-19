using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemIn", menuName = "Inventory/Item")]
public class ItemIn : ScriptableObject
{

    public enum ItemType
    {
        Consumbles, Weapon, SkillBase, SkillBoss
    }
    public Sprite image;
    public string Name;
    public ItemType itemType;

    public int Consumbles_Id;
    public int Consumbles_Value;

    public int Weapon_Id;
    public Vector3 Weapon_Range;
    public int Weapon_Damge;

    public int SkillBase_Id;

    public int SkillBoss_Id;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
