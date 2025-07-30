using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMouse : MonoBehaviour
{
    public static bool CursorLocked = true;
    [SerializeField] protected KeyCode lockMouseKey = KeyCode.LeftAlt;

    Animator animator;

    [SerializeField] internal GameObject inventory;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        inventory.SetActive(false);
        animator = GetComponent<Animator>();
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

            if (Input.GetKeyDown(lockMouseKey))
            {
                CursorLocked = false;
                GetComponent<MoveManager>().CheckLockMove(true);
                GetComponent<MoveManager>().CheckDrag(true);
                inventory.SetActive(true);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(lockMouseKey))
            {
                CursorLocked = true;
                GetComponent<MoveManager>().CheckLockMove(false);
                GetComponent<MoveManager>().CheckDrag(false);
                inventory.SetActive(false);
            }
        }
    }
}
