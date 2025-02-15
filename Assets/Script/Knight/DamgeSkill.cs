using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DamgeSkill : MonoBehaviour
{
    public Transform pointAttack;
    public float radius;
    public LayerMask attackMask;
    public int stunDamge;
    public int damge;
    int Damge;
    // Start is called before the first frame update
    void Start()
    {
        Damge1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damge1()
    {
        Damge = damge;
        Attack();
    }
    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapSphere(pointAttack.position, radius, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<EnemyTakeDamge>().TakeDamge(Damge, stunDamge);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointAttack.position, radius);
    }
}
