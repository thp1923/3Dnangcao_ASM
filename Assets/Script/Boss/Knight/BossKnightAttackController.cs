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

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        this.enabled = false;
    }
    private void OnEnable()
    {
        if (animator == null) animator = GetComponent<Animator>();

        animator.applyRootMotion = true;

        if (hasFired) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);
        if (hits.Length > 0)
        {
            int index = GetRandomIndex();
            if (triggers.Length > 0 && index < triggers.Length)
                animator.SetTrigger(triggers[index].triggerName);
            hasFired = true;
        }
    }

    private void OnDisable()
    {
        ResetTrigger();
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
    }
}
