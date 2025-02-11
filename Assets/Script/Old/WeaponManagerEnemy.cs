using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagerEnemy : MonoBehaviour
{
    public GameObject weapon;
    public void EnableWeapon(int isEnable)
    {
        if (weapon != null)
        {
            var col = weapon.GetComponent<Collider>();
            if (isEnable == 1)
            {
                col.enabled = true;
            }
            else
            {
                col.enabled = false;
            }
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
