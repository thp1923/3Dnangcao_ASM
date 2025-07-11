using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionKey : MonoBehaviour
{
    public bool hasKey = false;
    public float interactDistance = 2f;
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
                    Debug.Log("Door unlocked!");
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
            Debug.Log("Key collected!");
        }
    }
}
