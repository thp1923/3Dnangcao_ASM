using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTransState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerTakeDamge>().isBlock = false;
        animator.GetComponent<PlayerTakeDamge>().BlockEffect(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MoveManager>().CheckLockMove(true);
        if (!Input.GetMouseButtonDown(1) && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetKey(KeyCode.LeftShift)))
        {
            animator.SetBool("hasInput", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MoveManager>().CheckLockMove(false);
        animator.GetComponent<MoveManager>().CheckSleep(false);
        animator.GetComponent<PlayerTakeDamge>().PlayFlame(false);
        animator.GetComponent<Stamina>().canRecover = true;
        animator.SetBool("hasInput", false);
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
