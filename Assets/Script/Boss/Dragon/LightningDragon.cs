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

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        transform.position = player.position;
        lightning?.Play();
        StartCoroutine(Hide());

        // Nếu muốn spawn ra là đánh luôn thì để lại dòng này,
        // còn không thì bỏ đi, để gọi Attack() từ bên ngoài
        Attack(0);
    }

    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        LightningDamage(attackNumber);
    }

    public void LightningDamage(int attackNum)
    {
        if (stunDamge == null || attackNum < 0 || attackNum >= stunDamge.Length) return;

        Collider[] hits = Physics.OverlapBox(transform.position, lightningRange, Quaternion.identity, attackMask);

        foreach (Collider hit in hits)
        {
            PlayerTakeDamge takeDamage = hit.GetComponent<PlayerTakeDamge>();
            if (takeDamage != null)
            {
                takeDamage.TakeDamge(atk, stunDamge[attackNum], 0);
            }
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
        Gizmos.DrawWireCube(transform.position, lightningRange);
    }
}
