using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInteractionKey : MonoBehaviour
{
    public bool hasKey = false;
    public float interactDistance = 2f;
    private DoorUnlock nearbyDoor;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = new Ray(transform.position,transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                DoorUnlock door = hit.collider.GetComponent<DoorUnlock>();
                if (door != null && door.isLocked && hasKey)
                {
                    door.UnlockDoor();
                    //Debug.Log("Door unlocked!");
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (nearbyDoor != null && nearbyDoor.isLocked)
                {
                    if (hasKey)
                    {
                        nearbyDoor.UnlockDoor();
                        UIDoorMes.instance.HideMessage();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(other.gameObject);
            //Debug.Log("Key collected!");
            if (other.GetComponent<DoorUnlock>() != null)
            {
                nearbyDoor = null;
                UIDoorMes.instance.HideMessage();
            }
        }

        if (other.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(other.gameObject);
            UIDoorMes.instance.HideMessage();
        }
        else if (other.GetComponent<DoorUnlock>() != null)
        {
            nearbyDoor = other.GetComponent<DoorUnlock>();
            Debug.Log("Player entered door trigger.");
            if (nearbyDoor.isLocked)
            {
                if (hasKey)
                {
                    UIDoorMes.instance.ShowMessage("Ấn để mở khóa");
                }
                else
                {
                    UIDoorMes.instance.ShowMessage("Cần chìa");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DoorUnlock>() != null && other.GetComponent<DoorUnlock>() == nearbyDoor)
        {
            nearbyDoor = null;
            UIDoorMes.instance.HideMessage();
        }
    }
}

