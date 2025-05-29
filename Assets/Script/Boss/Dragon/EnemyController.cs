using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform player;
    Animator animator;

    public bool isAttacking;
    public bool noChange;

    public float rotationSpeed = 3f;
    public float viewAngleThreshold = 45f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        HandleTurning();
    }

    void HandleTurning()
    {
        // Tính hướng tới player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        float signedAngle = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);
        if (angle > viewAngleThreshold)
        {
            noChange = true;
            animator.SetBool("Turn", true);
        }
        else
        {
            noChange = false;
            animator.SetBool("Turn", false);
        }

        Debug.Log("Angle:" + angle);
        if (player == null) return;

        if(isAttacking) return;

        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //// Kiểm tra nếu đang ở trạng thái "Turn" (tên bạn đặt trong Animator)
        //if (!stateInfo.IsName("Turn"))
        //{
        //    // Không ở trong state cho phép xoay → không quay
        //    animator.SetFloat("TurnAngle", 0f);
        //    return;
        //}

        if (angle > viewAngleThreshold)
        {
            // Blend animation quay trái/phải
            float normalizedAngle = Mathf.Clamp(signedAngle / 90f, -1f, 1f);
            
            animator.SetFloat("TurnAngle", normalizedAngle);

            // Quay enemy
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            
            // Reset Blend Tree nếu không cần quay
            animator.SetFloat("TurnAngle", 0f);
        }
    }
}
