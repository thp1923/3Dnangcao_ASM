using UnityEngine;
using UnityEngine.AI;

public class IdleDragonBehaviour : StateMachineBehaviour
{
    enum TypeAttack
    {
        Base,
        Combo
    }
    TypeAttack typeAttack;

    public float[] attackChange;

    public float[] comboChange;

    public float[] typeChange;

    bool isChange;

    public float chaseDistance;

    public float attackRange;

    public float comboDistance;

    NavMeshAgent agent;



    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isChange = false;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if(distance <= attackRange && !isChange)
        {
            if(distance < comboDistance)
            {
                typeChange[0] -= 50;
            }
            else
            {
                typeChange[0] = 70;
            }
            float ramdomType = Random.Range(0, 100f);
            if(ramdomType < typeChange[0])
            {
                typeAttack = TypeAttack.Base;
            }
            else if(ramdomType < typeChange[1])
            {
                typeAttack = TypeAttack.Combo;
            }
            switch(typeAttack)
            {
                case TypeAttack.Base:
                    float randomAttack = Random.Range(0, 100f);
                    if (randomAttack < attackChange[0])
                    {
                        animator.SetTrigger("Attack" + 1);
                    }
                    else if (randomAttack < attackChange[1])
                    {
                        animator.SetTrigger("Attack" + 2);
                    }
                    else if (randomAttack < attackChange[2])
                    {
                        animator.SetTrigger("Attack" + 3);
                    }
                    else if (randomAttack < attackChange[3])
                    {
                        animator.SetTrigger("Attack" + 4);
                    }
                    else if (randomAttack < attackChange[4])
                    {
                        animator.SetTrigger("Attack" + 5);
                    }
                    break;
                case TypeAttack.Combo:
                    float randomCombo = Random.Range(0, 100f);
                    if(randomCombo < comboChange[0])
                    {
                        animator.SetTrigger("Combo" + 1);
                    }
                    else if (randomCombo < comboChange[1])
                    {
                        animator.SetTrigger("Combo" + 2);
                    }
                    else if (randomCombo < comboChange[2])
                    {
                        animator.SetTrigger("Combo" + 3);
                    }
                    else if (randomCombo < comboChange[3])
                    {
                        animator.SetTrigger("Combo" + 4);
                    }
                    break;
                default: break;
            }
            isChange = true;
        }
        if(distance <= chaseDistance && !isChange)
        {
            animator.SetBool("IsChase", true);
            isChange = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
