using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackDamgePlayer : MonoBehaviour
{
    public Transform pointAttack;
    public Transform pointAttack2;
    public float radius;
    public LayerMask attackMask;
    public int damge1;
    public int damge2;
    public int damge3;
    int Damge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damge1()
    {
        Damge = damge1;
        Attack();
    }
    public void Damge2()
    {
        Damge = damge1;
        Attack();
    }
    public void Damge3()
    {
        Damge = damge1;
        Attack2();
    }
    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapSphere(pointAttack.position, radius, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<EnemyTakeDamge>().TakeDamge(Damge);
        }
    }
    public void Attack2()
    {
        Collider[] colInfo = Physics.OverlapSphere(pointAttack2.position, radius, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<EnemyTakeDamge>().TakeDamge(Damge);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointAttack.position, radius);
        Gizmos.DrawWireSphere(pointAttack2.position, radius);
    }
}
