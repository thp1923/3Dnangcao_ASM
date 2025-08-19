using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRunBehaviour : StateMachineBehaviour
{
    public Vector3 attackRangeAdd;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerAttackController>().canClick = false;
        animator.GetComponent<MoveManager>().CheckLockMove(true);
        animator.GetComponent<Stamina>().TakeStamina(animator.GetComponent<PlayerAttackController>().staminaLost);
        animator.GetComponent<SwordTrailEffect>().PlayFlame(true);
        animator.GetComponent<AttackDamgePlayer>().attackRange += attackRangeAdd;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MoveManager>().CheckSleep(true);
        animator.GetComponent<AudioPlayer>().source.Stop();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("AttackRun");
        animator.GetComponent<AttackDamgePlayer>().attackRange -= attackRangeAdd;
        animator.GetComponent<PlayerAttackController>().canClick = true;
        animator.GetComponent<PlayerAttackController>().inputRecceived = false;
        animator.GetComponent<PlayerAttackController>().ResetAttack();
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
