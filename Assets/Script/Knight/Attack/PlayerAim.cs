using Invector.vCharacterController;
using System.Collections;
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

    private List<GameObject> enemiesList = new List<GameObject>();
    private GameObject closestEnemy;
    private bool lockOn;
    private vThirdPersonController tcp;

    void Start()
    {
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

        if (lockCol.Length > 0)
        {
            if (Input.GetKeyDown(lockKey))
            {
                lockOn = !lockOn;

                if (lockOn)
                {
                    closestEnemy = null;
                    ClosestEnemy(); // lock vào mục tiêu gần nhất
                }
                else
                {
                    closestEnemy = null;
                }
            }

            if (lockOn)
            {
                float horizontalMouse = Input.GetAxis("Mouse X");

                if (horizontalMouse > 2f)
                {
                    SwitchTarget(true); // sang phải
                }
                else if (horizontalMouse < -2f)
                {
                    SwitchTarget(false); // sang trái
                }
            }
        }
        else
        {
            lockOn = false;
            closestEnemy = null;
            tcp.Strafe();
        }
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

            // Xoay camera mượt
            cam.transform.position = camTransLock.position;
            Quaternion camTargetRotation = Quaternion.LookRotation(directionToEnemy);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camTargetRotation, rotationSpeedCamera * Time.deltaTime);

            cam.GetComponent<vThirdPersonCamera>().target = null;
        }
        else
        {
            cam.GetComponent<vThirdPersonCamera>().target = transform;
        }
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

    public void RemoveEnemy()
    {
        if (closestEnemy != null)
        {
            enemiesList.Remove(closestEnemy);
        }
    }
}
