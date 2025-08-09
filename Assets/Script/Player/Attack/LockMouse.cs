using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMouse : MonoBehaviour
{
    public static bool CursorLocked = true;


    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateCursorLock();
    }
    public void UpdateCursorLock()
    {
        if (CursorLocked)
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

    public void LockController(bool Lock)
    {
        if (!Lock)
        {
            CursorLocked = false;
            GetComponent<MoveManager>().CheckLockMove(true);
            GetComponent<MoveManager>().CheckDrag(true);
        }
        else
        {
            CursorLocked = true;
            GetComponent<MoveManager>().CheckLockMove(false);
            GetComponent<MoveManager>().CheckDrag(false);
        }
    }
}
