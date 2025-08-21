using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMouseLock : MonoBehaviour
{
    public bool Lock;
    // Start is called before the first frame update
    void Start()
    {
        if (Lock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
