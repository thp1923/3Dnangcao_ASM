using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectWolf : MonoBehaviour
{
    public CapsuleCollider weapon;
    public CapsuleCollider weapon2;
    // Start is called before the first frame update
    void Start()
    {
        
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
    public void WeaponColider2(int number)
    {
        if (number == 0) weapon2.enabled = true;
        else weapon2.enabled = false;
    }
}
