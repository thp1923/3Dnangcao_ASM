using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIController : MonoBehaviour
{

    public ItemIn item;
    public void SetItem(ItemIn item)
    {
        this.item = item;
    }

    public void Remove()
    {
        //InvetoryManager.Instance.Remove(item/*, itemModel*/);
        FindObjectOfType<InvetoryManager>().Remove(item/*, itemModel*/);
        Destroy(this.gameObject);
    }

    public void UseItem()
    {
        switch(item.itemType)
        {
            case ItemIn.ItemType.Consumbles:
                break;
            case ItemIn.ItemType.Weapon:
                break;
            case ItemIn.ItemType.SkillBase:
                break;
            case ItemIn.ItemType.SkillBoss:
                break;
            default:
                break;
        }
        Remove();
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
