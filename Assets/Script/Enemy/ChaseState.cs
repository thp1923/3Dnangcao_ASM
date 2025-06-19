using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    public float playerDistance;
    public float attackRange;
    Transform player;
    public float timer;
    float _timer;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer -= Time.deltaTime;
        if (FindObjectOfType<PlayerTakeDamge>().isDeath)
        {
            animator.SetBool("IsRunning", false);
            return;
        }
        float distance = Vector3.Distance(player.position, animator.transform.position);
        animator.transform.LookAt(player);
        if (distance <= attackRange)
            animator.SetTrigger("Attack");
        if (distance > playerDistance && _timer <= 0)
        {
            animator.SetBool("IsChasing", false);
            _timer = timer;
        } 
        else if (distance > attackRange && distance < playerDistance && _timer <= 0)
        {
            animator.SetBool("IsRunning", true);
            _timer = timer;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        
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
