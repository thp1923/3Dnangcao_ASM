using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDistance : MonoBehaviour
{
    public float chaseDistance;
    public float attackDistance;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(gameObject.transform.position, attackDistance);
        Gizmos.DrawWireSphere(gameObject.transform.position, chaseDistance);
    }
}
