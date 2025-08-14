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
        
    }

    private void OnEnable()
    {
        StartCoroutine(Hide());
        StartCoroutine(ExplosionDamge());
        trueDamge = (int)(BaseATK * 1.2f);
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

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(timerDestroy);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
