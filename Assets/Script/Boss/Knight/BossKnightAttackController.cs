using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct WeightedTrigger
{
    public string triggerName;
    public float weight;
}

public class BossKnightAttackController : MonoBehaviour
{
    public float detectRadius = 3f;
    public LayerMask playerLayer;
    public WeightedTrigger[] triggers;

    private bool hasFired = false;
    private bool hasUpdate = false;

    private NavMeshAgent agent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        this.enabled = false;
    }
    private void OnEnable()
    {
        if (hasFired) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);
        if (hits.Length > 0)
        {
            int index = GetRandomIndex();
            animator.SetTrigger(triggers[index].triggerName);
            hasFired = true;
        }
        else
        {
            StartCoroutine(CheckAttack());
        }
    }

    private void Update()
    {
        if (hasFired && !hasUpdate)
        {
            hasUpdate = true;
            StartCoroutine(CheckAttack());
        }
    }

    private void OnDisable()
    {
        ResetTrigger();
    }

    IEnumerator CheckAttack()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<BossKnightAttackController>().enabled = false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    int GetRandomIndex()
    {
        float total = 0;
        foreach (var t in triggers)
            total += t.weight;

        float rand = Random.Range(0f, total);
        float sum = 0;

        for (int i = 0; i < triggers.Length; i++)
        {
            sum += triggers[i].weight;
            if (rand <= sum)
                return i;
        }

        return 0;
    }

    // Optional: Reset lại nếu muốn tái sử dụng sau này
    public void ResetTrigger()
    {
        hasFired = false;
        hasUpdate = false;
        agent.enabled = true;
        animator.SetBool("IsMoving", true);
        GetComponent<BossKnightMoveAI>().enabled = true;
        int index = GetRandomIndex();
        animator.ResetTrigger(triggers[index].triggerName);
    }
}
