using UnityEngine;

public class HitState : StateMachineBehaviour
{
    public bool Hit3;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsStun", true);
        animator.GetComponent<PlayerTakeDamge>().noTakeDamge = true;
        animator.GetComponent<MoveManager>().CheckLockMove(true);
        animator.GetComponent<MoveManager>().CheckSleep(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsStun", false);
        ResetAllTriggers(animator);
        if (Hit3) return;
        animator.GetComponent<PlayerTakeDamge>().noTakeDamge = false;
        animator.GetComponent<MoveManager>().CheckLockMove(false);
        animator.GetComponent<MoveManager>().CheckSleep(false);
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
