using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;

public class PatrollState : StateMachineBehaviour
{
    public float time;
    public float speed;
    public float chaseDistance;
    float timer;
    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;
    Transform player;

    public int index;
    public int sourceIndex;
    public float pitch;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        timer = 0;
        agent.speed = speed;
        animator.GetComponent<PlayAudioEnemy>().PlayAlwaysUpPitch(index, pitch);
        WayPoint wayPointScript = animator.GetComponent<WayPoint>();
        foreach (Transform wayPoint in wayPointScript.WayPoints)
        {
            wayPoints.Add(wayPoint);
        }
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!agent.enabled)
            return;

        if(agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        timer += Time.deltaTime;
        if (timer > time)
            animator.SetBool("IsPatrolling", false);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance <= chaseDistance)
        {
            animator.SetBool("IsChasing", true);
        }
        if(distance >= (chaseDistance + 5))
        {
            animator.GetComponent<PlayAudioEnemy>().PlayAudioStop(index);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayAudioEnemy>().ResetPitch();
        animator.GetComponent<PlayAudioEnemy>().PlayAudioStop(index);
        if (!agent.enabled)
            return;
        agent.SetDestination(agent.transform.position);
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
