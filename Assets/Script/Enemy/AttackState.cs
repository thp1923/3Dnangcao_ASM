using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    public float attackRange;
    NavMeshAgent agent;
    Transform player;
    bool isCheck;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.enabled = false;
        isCheck = false;
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

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance <= attackRange)
            animator.SetBool("IsRunning", false);
        else
        {
            if (isCheck) return;
            float random = UnityEngine.Random.Range(-1f, 1f);
            if(random > 0)
            {
                animator.SetBool("IsRunning", true); 
                isCheck = true;
            }
            else
            {
                animator.SetBool("IsRunning", false);
                isCheck = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
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
