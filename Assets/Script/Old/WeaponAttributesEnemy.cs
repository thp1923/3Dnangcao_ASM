using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributesEnemy : MonoBehaviour
{
    public AttributesManager atm;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<AttributesManager>().TakeDamge(atm.Attack);
        }
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
