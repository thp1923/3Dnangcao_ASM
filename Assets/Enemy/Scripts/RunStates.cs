using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RunStates : StateMachineBehaviour
{
 
    public class ChaseState : StateMachineBehaviour
    {
        NavMeshAgent agent;
        Transform player;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // tìm player
            player = GameObject.FindGameObjectWithTag("Player").transform;

            // truy tới NavMeshAgent
            agent = animator.gameObject.GetComponent<NavMeshAgent>();
            agent.speed = 2f;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // gán đích đến là vị trí của player
            agent.SetDestination(player.position);

            float distance = Vector3.Distance(player.position, animator.transform.position);
            if (distance < 2.5f)
                animator.SetBool("isAttacking", true);
            else
                animator.SetBool("isAttacking", false);

        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // khi rời khỏi trạng thái, enemy dừng lại
            agent.SetDestination(animator.transform.position);
        }
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
