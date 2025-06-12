using UnityEngine;

public class InvectorLockOnController : MonoBehaviour
{
    public LockOnTarget targetSystem;
    public vThirdPersonCamera tpCamera;
    public Transform defaultTarget; // usually the Player
    private bool isLocked = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isLocked)
            {
                Transform t = targetSystem.GetNearestTarget();
                if (t != null)
                {
                    tpCamera.SetTarget(t); // dùng đúng hàm của camera bạn
                    isLocked = true;
                }
            }
            else
            {
                tpCamera.SetTarget(defaultTarget);
                isLocked = false;
            }
        }

        if (isLocked)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Transform t = targetSystem.SwitchTarget(false);
                if (t != null) tpCamera.SetTarget(t);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Transform t = targetSystem.SwitchTarget(true);
                if (t != null) tpCamera.SetTarget(t);
            }

            // Player auto-rotate toward enemy
            if (targetSystem.currentTarget != null)
            {
                Vector3 direction = (targetSystem.currentTarget.position - transform.position).normalized;
                direction.y = 0;
                if (direction.magnitude > 0.1f)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                }
            }
        }
    }
}