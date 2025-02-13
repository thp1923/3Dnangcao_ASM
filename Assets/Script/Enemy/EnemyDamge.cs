using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamge : MonoBehaviour
{
    public Transform pointAttack;
    public float radius;
    public int stunDamge;
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
        Attack();
    }
    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapSphere(pointAttack.position, radius, attackMask);
        foreach (Collider player in colInfo)
        {
            player.GetComponent<PlayerTakeDamge>().TakeDamge(Damge, stunDamge);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointAttack.position, radius);
    }
}
