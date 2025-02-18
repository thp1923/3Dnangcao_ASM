using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoDamge : MonoBehaviour
{
    public LayerMask attackMask;
    public Vector3 box;
    public Transform point;
    public int damge1;
    public int stunDamge;
    public int knockBack;
    int Damge;

    bool isAttack;

    public string tagAttack;

    public int attackCount;
    public int attackMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(attackCount >= attackMax)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagAttack))
        {
            isAttack = true;
            Damge1();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagAttack))
        {
            isAttack = false;
        }
    }

    public void Damge1()
    {
        Damge = damge1;
        Attack();
    }

    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapBox(point.position, box, Quaternion.identity, attackMask);
        foreach (Collider player in colInfo)
        {
            if (!isAttack) return;
            attackCount++;
            Debug.Log("attack" + attackCount);
            if (player.GetComponent<PlayerTakeDamge>().isBlock)
            {
                Destroy(gameObject);
                return;
            }
            Invoke(nameof(Damge1), 0.2f);
            player.GetComponent<PlayerTakeDamge>().TakeDamge(Damge, stunDamge, knockBack);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(point.position, box);
    }
}
