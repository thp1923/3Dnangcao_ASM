using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderWeapon : MonoBehaviour
{
    public BoxCollider weapon;
    // Start is called before the first frame update
    void Start()
    {
        weapon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WeaponColider(int number)
    {
        if (number == 0) weapon.enabled = true;
        else weapon.enabled = false;
    }
}
