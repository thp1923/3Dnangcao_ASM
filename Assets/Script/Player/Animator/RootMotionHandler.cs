using UnityEngine;

public class RootMotionHandler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorMove()
    {
        if(animator.GetFloat("InputMagnitude") > 0 && !GetComponent<PlayerAttackController>().isAttacking) 
            return;
        // Kiểm tra nếu root motion đang được xử lý bằng script
        if (animator.applyRootMotion)
        {
            // Áp dụng vị trí và xoay từ animation
            transform.position += animator.deltaPosition;
            transform.rotation *= animator.deltaRotation;
        }
    }
}
