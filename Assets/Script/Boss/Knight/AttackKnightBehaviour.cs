using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackKnightBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<BossKnightMoveAI>().enabled = false;
        if (animator.GetComponent<SwordTrailEffect>() != null)
        {
            animator.GetComponent<SwordTrailEffect>().PlayFlame(true);
            animator.GetComponent<SwordTrailEffect>().PlayPartical(1);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<BossKnightAttackController>().enabled != false)
            animator.GetComponent<BossKnightAttackController>().enabled = false;
        if (animator.GetComponent<BossKnightMoveAI>().enabled != false)
            animator.GetComponent<BossKnightMoveAI>().enabled = false;
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetAllTriggers(animator);
        if (animator.GetComponent<SwordTrailEffect>() != null)
        {
            animator.GetComponent<SwordTrailEffect>().PlayFlame(false);
            animator.GetComponent<SwordTrailEffect>().PlayPartical(1);
        }
        if (animator.GetComponent<BossKnightAttackController>().enabled != false)
            animator.GetComponent<BossKnightAttackController>().enabled = false;
        if(animator.GetComponent<BossKnightMoveAI>().enabled != true)
            animator.GetComponent<BossKnightMoveAI>().enabled = true;
    }

    void ResetAllTriggers(Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
