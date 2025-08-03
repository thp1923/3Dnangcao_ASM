using Invector.vCharacterController;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Aim Settings")]
    public float lockRange = 10f;
    public LayerMask enemyLayer;
    public KeyCode lockKey = KeyCode.Tab;
    public Camera cam;
    public Transform camTransLock;
    public float rotationSpeedCharacter = 5f;
    public float rotationSpeedCamera = 7f;
    public float cameraMoveSpeed = 5f;
    Animator animator;

    private List<GameObject> enemiesList = new List<GameObject>();
    private GameObject closestEnemy;
    private bool lockOn = false;
    private bool transitioningFromLock = false;
    private vThirdPersonController tcp;

    public GameObject lockIcon;
    Transform child;

    void Start()
    {
        lockIcon.SetActive(false);
        animator = GetComponent<Animator>();
        tcp = GetComponent<vThirdPersonController>();
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemiesInScene)
        {
            enemiesList.Add(enemy);
        }
    }

    void Update()
    {
        Collider[] lockCol = Physics.OverlapSphere(transform.position, lockRange, enemyLayer);

        if (lockCol.Length > 0 && !GetComponent<PlayerTakeDamge>().isDeath)
        {
            if (Input.GetKeyDown(lockKey))
            {
                lockOn = !lockOn;

                if (lockOn)
                {
                    closestEnemy = null;
                    ClosestEnemy();
                    transitioningFromLock = false;
                }
                else
                {
                    closestEnemy = null;
                    transitioningFromLock = true;
                }
            }

            if (lockOn)
            {
                float horizontalMouse = Input.GetAxis("Mouse X");

                if (horizontalMouse > 2f)
                {
                    SwitchTarget(true);
                }
                else if (horizontalMouse < -2f)
                {
                    SwitchTarget(false);
                }
            }
        }
        else
        {
            if (lockOn)
            {
                lockOn = false;
                closestEnemy = null;
                transitioningFromLock = true;
            }
            if (animator.GetBool("IsStrafing"))
            {
                tcp.Strafe(); // giữ cho nhân vật vẫn hoạt động bình thường khi không lock
            }
        }
        Icon();
    }

    void Icon()
    {
        if (!lockOn || closestEnemy == null)
        {
            lockIcon.SetActive(false);
            return;
        }
        child = closestEnemy.transform.Find("LockPoint");
        if (child == null)
        {
            lockIcon.SetActive(false);
            return;
        }
        lockIcon.transform.position = child.position;
        lockIcon.SetActive(true);
    }

    void LateUpdate()
    {
        if (lockOn && closestEnemy != null)
        {
            Vector3 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
            directionToEnemy.y = 0;


            // Xoay nhân vật mượt
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeedCharacter * Time.deltaTime);

            // Di chuyển camera đến vị trí lock mượt
            cam.transform.position = Vector3.MoveTowards(
                cam.transform.position,
                camTransLock.position,
                cameraMoveSpeed * Time.deltaTime
            );

            // Xoay camera mượt
            Quaternion camTargetRotation = Quaternion.LookRotation(directionToEnemy);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camTargetRotation, rotationSpeedCamera * Time.deltaTime);

            cam.GetComponent<vThirdPersonCamera>().target = null;
        }
        else if (transitioningFromLock)
        {
            // Vị trí camera mặc định
            Vector3 playerViewOffset = transform.position + new Vector3(0, 1.8f, -3f); // điều chỉnh theo góc mặc định
            cam.transform.position = Vector3.MoveTowards(
                cam.transform.position,
                playerViewOffset,
                cameraMoveSpeed * Time.deltaTime
            );

            // === Cập nhật: Xoay mượt camera để nhìn về phía nhân vật ===
            Vector3 lookDirection = (transform.position + Vector3.up * 1.5f) - cam.transform.position;
            Quaternion camTargetRotation = Quaternion.LookRotation(lookDirection.normalized);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camTargetRotation, rotationSpeedCamera * Time.deltaTime);

            if (Vector3.Distance(cam.transform.position, playerViewOffset) < 0.1f)
            {
                cam.GetComponent<vThirdPersonCamera>().target = transform;
                transitioningFromLock = false;
            }
            else
            {
                cam.GetComponent<vThirdPersonCamera>().target = null;
            }
        }
        else
        {
            // Trạng thái bình thường ban đầu
            cam.GetComponent<vThirdPersonCamera>().target = transform;
        }
    }

    public void LockForStun()
    {
        float range = lockRange;
        foreach (GameObject enemy in enemiesList)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < range)
            {
                range = dist;
                closestEnemy = enemy;
            }
        }
        if (closestEnemy == null) return;

        Vector3 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
        directionToEnemy.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
        transform.rotation = targetRotation; // Xoay ngay lập tức
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, lockRange);
    }

    public void ClosestEnemy()
    {
        if (!lockOn || closestEnemy != null) return;

        float range = lockRange;
        foreach (GameObject enemy in enemiesList)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < range)
            {
                range = dist;
                closestEnemy = enemy;
            }
        }
        
    }

    private void SwitchTarget(bool right)
    {
        GameObject bestTarget = null;
        float bestAngle = 180f;

        foreach (GameObject enemy in enemiesList)
        {
            if (enemy == null || enemy == closestEnemy) continue;

            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(transform.forward, dirToEnemy, Vector3.up);

            if ((right && angle > 5f) || (!right && angle < -5f))
            {
                float absAngle = Mathf.Abs(angle);
                if (absAngle < bestAngle)
                {
                    bestAngle = absAngle;
                    bestTarget = enemy;
                }
            }
        }

        if (bestTarget != null)
        {
            closestEnemy = bestTarget;
        }
        
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            enemiesList.Remove(enemy);
        }
    }
}
