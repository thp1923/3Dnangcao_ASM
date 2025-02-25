using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMouse : MonoBehaviour
{
    public static bool CursorLocked = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCursorLock()
    {
        if (CursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //UnlockMove();
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                CursorLocked = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //LockMove();


            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                CursorLocked = true;
            }
        }
    }
}
