using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAIMove : MonoBehaviour
{
    Transform player;
    public float detectRadius = 16f;
    public float attackRange = 4f;
    public LayerMask playerLayer;


    public float strafeRadius = 3f;
    public float strafeSpeed = 50f;
    public float switchInterval = 3f;

    private NavMeshAgent agent;
    private Animator animator;

    private float angle;
    private float switchTimer;
    private bool isStrafing = true;
    private bool playerInRange = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        angle = Random.Range(0f, 360f);

        agent.updateRotation = false; // tự xoay bằng code
    }

    private void OnEnable()
    {
        animator.applyRootMotion = false;
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }

    private void OnDisable()
    {
        agent.enabled = false;
        playerInRange = false;
    }

    void Update()
    {

        DetectPlayer();

        if (!playerInRange)
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("InputX", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("InputY", 0f, 0.1f, Time.deltaTime);
            return;
        }
        if (!agent.enabled) return;
        agent.isStopped = false;

        switchTimer += Time.deltaTime;
        if (switchTimer > switchInterval)
        {
            isStrafing = !isStrafing;
            switchTimer = 0f;
        }

        if (isStrafing)
            StrafeAroundPlayer();
        else
            ChasePlayer();

        UpdateAnimator();
        LookAtPlayer();
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);
        playerInRange = hits.Length > 0;
        Collider[] Range = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
        if (Range.Length > 0)
        {
            GetComponent<BossKnightAttackController>().enabled = true;
            playerInRange = false;
        }
    }

    void StrafeAroundPlayer()
    {
        angle += strafeSpeed * Time.deltaTime;
        if (angle > 360f) angle -= 360f;

        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * strafeRadius;
        Vector3 targetPos = player.position + offset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            if (!GetComponent<BossAIMove>().enabled) return;
            agent.SetDestination(hit.position);
        }
    }

    void ChasePlayer()
    {
        if (!GetComponent<BossAIMove>().enabled) return;
        agent.SetDestination(player.position);
    }

    void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        float speed = velocity.magnitude;

        animator.SetBool("IsMoving", speed > 0.1f);

        if (speed > 0.05f)
        {
            Vector3 localVel = transform.InverseTransformDirection(velocity);
            float inputX = Mathf.Clamp(localVel.x / agent.speed, -1f, 1f);
            float inputY = Mathf.Clamp(localVel.z / agent.speed, -1f, 1f);

            animator.SetFloat("InputX", inputX, 0.1f, Time.deltaTime);
            animator.SetFloat("InputY", inputY, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("InputX", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("InputY", 0f, 0.1f, Time.deltaTime);
        }
    }

    void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    // Debug draw sphere in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
