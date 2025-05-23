using System.Collections;
using UnityEngine;
using StatsManager;

public class LightningDragon : StatsAttack
{
    Transform player;
    public float timeHide = 1.5f;
    ParticleSystem lightning;

    public Vector3 lightningRange;
    public LayerMask attackMask;

    private void Awake()
    {
        lightning = GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.position;
        lightning.Play();
        StartCoroutine(Hide());
        Attack(0);
    }

    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        LightningDamge(attackNumber);
    }

    public void LightningDamge(int attackNum)
    {
        Collider[] col = Physics.OverlapBox(transform.position, lightningRange, Quaternion.identity, attackMask);
        foreach(Collider player in col)
        {
            player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, stunDamge[attackNum], 0);
        }
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(timeHide);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(gameObject.transform.position, lightningRange);
    }
}
