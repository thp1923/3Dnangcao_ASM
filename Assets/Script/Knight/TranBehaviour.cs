using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class TranBehaviour : StateMachineBehaviour
{
    public int currentAttack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerAttackController.Instance.canRecceiveInput = true;
        animator.GetComponent<SwordTrailEffect>().PlayPartical(1);
        animator.GetComponent<MoveManager>().CheckLockMove(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MoveManager>().CheckSleep(true);
        if (PlayerAttackController.Instance.inputRecceived && !animator.GetComponent<PlayerDodge>().isDodging)
        {
            animator.SetTrigger("Attack" + currentAttack);
            PlayerAttackController.Instance.InputManager();
            //PlayerAttackController.Instance.inputRecceived = false;
            PlayerAttackController.Instance.isAttacking = true;
        }
        else if (!Input.GetMouseButtonDown(0) && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetKey(KeyCode.LeftShift)))
        {
            animator.SetBool("hasInput", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerAttackController.Instance.inputRecceived = false;
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
