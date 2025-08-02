using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastAttack : MonoBehaviour
{
    Animator animator;
    public float rateQuickAttack = 50f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void QuickAttack()
    {
        if(Random.Range(0, 100f) <= rateQuickAttack)
        {
            animator.SetTrigger("QuickAttack");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
