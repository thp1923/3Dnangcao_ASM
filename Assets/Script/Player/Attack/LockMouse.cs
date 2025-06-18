using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMouse : MonoBehaviour
{
    public static bool CursorLocked = true;
    [SerializeField] protected KeyCode lockMouseKey = KeyCode.LeftAlt;
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

            if (Input.GetKeyDown(lockMouseKey))
            {
                GetComponent<MoveManager>().CheckLockMove(true);
                GetComponent<MoveManager>().CheckDrag(true);
                CursorLocked = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(lockMouseKey))
            {
                GetComponent<MoveManager>().CheckLockMove(false);
                GetComponent<MoveManager>().CheckDrag(false);
                CursorLocked = true;
            }
        }
    }
}
