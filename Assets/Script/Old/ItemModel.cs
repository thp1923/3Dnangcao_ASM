using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemModel", menuName = "Inventory/ItemModel")]
public class ItemModel : ScriptableObject
{
    // Start is called before the first frame update
    public int Id;
    public GameObject itemModel;

    // Update is called once per frame
    void Update()
    {
        
    }
}
