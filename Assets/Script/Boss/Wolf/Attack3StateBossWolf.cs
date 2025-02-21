using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack3StateBossWolf : StateMachineBehaviour
{
    public float attackRange;
    NavMeshAgent agent;
    Transform player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayAudioEnemy>().PlayAudio(2);
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.enabled = false;
        animator.GetComponent<BossWolf>().Mp2 -= 100;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.GetComponent<PlayerTakeDamge>().isDeath)
        {
            return;
        }
        // Get the player's position and set the y-coordinate to zero.
        Vector3 playerPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        // Make the animator's transform look at the player's position.
        animator.transform.LookAt(playerPosition);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance <= attackRange)
            animator.SetBool("IsRunning", false);
        else
            animator.SetBool("IsRunning", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack3");
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
