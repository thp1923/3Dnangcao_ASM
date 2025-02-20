using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunStateBossWolf : StateMachineBehaviour
{
    public float playerDistance;
    public float attackRange;
    public float heavyAttackRange;
    public float skillAttackRange;
    public float speed;
    NavMeshAgent agent;
    Transform player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.speed = speed;
        animator.GetComponent<PlayAudioEnemy>().PlayAlwaysUpPitch(6, 0, 0.9f);
        if (player.GetComponent<PlayerTakeDamge>().isDeath)
            animator.SetBool("IsRunning", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent.enabled) return;
        if (FindObjectOfType<PlayerTakeDamge>().isDeath)
        {
            animator.SetBool("IsRunning", false);
            return;
        }
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance <= skillAttackRange && animator.GetComponent<BossWolf>().Mp2 >= 100)
            animator.SetTrigger("Attack3");
        else if (distance <= heavyAttackRange && distance >= 5 && animator.GetComponent<BossWolf>().Mp1 >= 50)
            animator.SetTrigger("Attack2");
        else if (distance <= attackRange)
            animator.SetTrigger("Attack1");
        if (distance > playerDistance)
            animator.SetBool("IsRunning", false);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayAudioEnemy>().ResetPitch(0);
        animator.GetComponent<PlayAudioEnemy>().PlayAudioStop(6, 0);
        if (!agent.enabled) return;
        agent.SetDestination(animator.transform.position);
        animator.SetBool("IsRunning", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
