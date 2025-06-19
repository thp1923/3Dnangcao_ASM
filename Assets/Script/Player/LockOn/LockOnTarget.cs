using UnityEngine;
using System.Collections.Generic;

public class LockOnTarget : MonoBehaviour
{
    public float lockOnRange = 15f;
    public LayerMask enemyLayer;
    public Transform currentTarget;
    public List<Transform> allTargets = new List<Transform>();
    public Transform player;

    public Transform GetNearestTarget()
    {
        allTargets.Clear();
        Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRange, enemyLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider enemy in enemies)
        {
            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            float distance = directionToEnemy.magnitude;

            if (Vector3.Angle(transform.forward, directionToEnemy) < 70f)
            {
                allTargets.Add(enemy.transform);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearest = enemy.transform;
                }
            }
        }

        currentTarget = nearest;
        return nearest;
    }

    public Transform SwitchTarget(bool toRight)
    {
        if (currentTarget == null || allTargets.Count <= 1) return currentTarget;

        Transform bestTarget = null;
        float bestAngle = toRight ? -360f : 360f;

        Vector3 dirToCurrent = (currentTarget.position - player.position).normalized;

        foreach (var enemy in allTargets)
        {
            if (enemy == currentTarget) continue;
            Vector3 dirToEnemy = (enemy.position - player.position).normalized;
            float angle = Vector3.SignedAngle(dirToCurrent, dirToEnemy, Vector3.up);

            if (toRight && angle > 5f && angle < bestAngle)
            {
                bestAngle = angle;
                bestTarget = enemy;
            }

            if (!toRight && angle < -5f && angle > bestAngle)
            {
                bestAngle = angle;
                bestTarget = enemy;
            }
        }

        if (bestTarget != null)
        {
            currentTarget = bestTarget;
            return bestTarget;
        }

        return currentTarget;
    }
}