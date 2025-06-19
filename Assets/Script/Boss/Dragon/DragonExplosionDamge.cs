using UnityEngine;
using StatsManager;
using System.Collections;

public class DragonExplosionDamge : StatsAttack
{
    public int trueDamge;
    public float radius;
    public LayerMask attackMask;

    public float timerDestroy;
    public float timer;

    private void Start()
    {
        Destroy(gameObject, timerDestroy);
        StartCoroutine(ExplosionDamge());
    }

    IEnumerator ExplosionDamge()
    {
        yield return new WaitForSeconds(timer);
        Collider[] col = Physics.OverlapSphere(gameObject.transform.position, radius, attackMask);
        foreach(Collider player in col)
        {
            player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, stunDamge[0], trueDamge);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
