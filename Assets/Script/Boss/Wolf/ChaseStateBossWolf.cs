using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStateBossWolf : StateMachineBehaviour
{
    public float playerDistance;
    public float attackRange;
    public float heavyAttackRange;
    public float skillAttackRange;
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.GetComponent<PlayerTakeDamge>().isDeath)
        {
            animator.SetBool("IsRunning", false);
            return;
        }
        // Get the player's position and set the y-coordinate to zero.
        Vector3 playerPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        // Make the animator's transform look at the player's position.
        animator.transform.LookAt(playerPosition);
        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (distance <= skillAttackRange && animator.GetComponent<BossWolf>().Mp2 >= 100)
            animator.SetTrigger("Attack3");
        else if (distance <= heavyAttackRange && distance >= 4 && animator.GetComponent<BossWolf>().Mp1 >= 50)
            animator.SetTrigger("Attack2");
        else if (distance <= attackRange)
            animator.SetTrigger("Attack1");
        if (distance > playerDistance)
            animator.SetBool("IsChasing", false);
        else if(distance >= attackRange)
            animator.SetBool("IsRunning", true);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
