using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunArtoriasBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<DragonRotation>().enabled != false)
            animator.GetComponent<DragonRotation>().enabled = false;
        if (animator.GetComponent<SwordTrailEffect>() != null)
        {
            animator.GetComponent<SwordTrailEffect>().PlayFlame(false);
            animator.GetComponent<SwordTrailEffect>().PlayPartical(0);
        }
        if (animator.GetComponent<ArtoriasAttackDamge>() != null)
        {
            animator.GetComponent<ArtoriasAttackDamge>().canAttack = false;
        }
        if(animator.GetComponent<BarukClawsTrail>() != null)
        {
            animator.GetComponent<BarukClawsTrail>().PlayParticalOff(0);
            animator.GetComponent<BarukClawsTrail>().PlayParticalOff(1);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<BossKnightAttackController>().enabled != false)
            animator.GetComponent<BossKnightAttackController>().enabled = false;
        if (animator.GetComponent<BossKnightMoveAI>().enabled != false)
            animator.GetComponent<BossKnightMoveAI>().enabled = false;
        
        
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
    bool HasParameter(Animator animator, string name, AnimatorControllerParameterType type)
    {
        foreach (var param in animator.parameters)
        {
            if (param.name == name && param.type == type)
                return true;
        }
        return false;
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (HasParameter(animator, "NotStun", AnimatorControllerParameterType.Bool))
        {
            animator.SetBool("NotStun", true);
        }
        ResetAllTriggers(animator);
        if (animator.GetComponent<SwordTrailEffect>() != null)
        {
            animator.GetComponent<SwordTrailEffect>().PlayFlame(false);
            animator.GetComponent<SwordTrailEffect>().PlayPartical(0);
        }
        if (animator.GetComponent<ArtoriasAttackDamge>() != null)
        {
            animator.GetComponent<ArtoriasAttackDamge>().canAttack = true;
        }
        if (animator.GetComponent<BossKnightAttackController>().enabled != false)
            animator.GetComponent<BossKnightAttackController>().enabled = false;
        if (animator.GetComponent<BossKnightMoveAI>().enabled != true)
            animator.GetComponent<BossKnightMoveAI>().enabled = true;
        if (animator.GetComponent<DragonRotation>().enabled != true)
            animator.GetComponent<DragonRotation>().enabled = true;
        if (animator.GetComponent<BarukClawsTrail>() != null)
        {
            animator.GetComponent<BarukClawsTrail>().PlayParticalOff(0);
            animator.GetComponent<BarukClawsTrail>().PlayParticalOff(1);
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
