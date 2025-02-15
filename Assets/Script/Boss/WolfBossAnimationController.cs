using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WolfBossAnimationController : MonoBehaviour
{
    [Header("Public")]
    public Animator animator;
    public float distanceMove;

    [Header("No Public")]
    Transform player;
    NavMeshAgent agent;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, gameObject.transform.position);
    }

    
}
