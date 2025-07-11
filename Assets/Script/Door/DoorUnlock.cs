using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    public bool isLocked = true;
    
    public void UnlockDoor()
    {
        if (isLocked)
        {
            isLocked = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}
