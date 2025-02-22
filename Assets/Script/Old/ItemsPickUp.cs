using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsPickUp : MonoBehaviour
{
    public ItemIn item;
    public ItemModel itemModel;
    void PickUp()
    {
        Destroy(gameObject);
        //InvetoryManager.Instance.Add(item/*, itemModel*/);
        FindObjectOfType<InvetoryManager>().Add(item/*, itemModel*/);
    }
    //private void OnMouseDown()
    //{
    //    PickUp();

    //}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }
}
