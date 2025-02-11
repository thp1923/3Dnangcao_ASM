using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject weapon;
    public vThirdPersonController tcp;

    public void EnableWeapon(int isEnable)
    {
        if(weapon != null)
        {
            var col = weapon.GetComponent<Collider>();
            if(isEnable == 1)
            {
                col.enabled = true;
            }
            else
            {
                col.enabled = false;
            }
        }
    }
    public void StopAttack()
    {
        tcp.lockMovement = true;
        tcp.lockRotation = true;
    }
    public void MoveAttack()
    {
        tcp.lockMovement = false;
        tcp.lockRotation = false;
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
