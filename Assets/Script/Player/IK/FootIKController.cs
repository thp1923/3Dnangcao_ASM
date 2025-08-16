using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootIKController : MonoBehaviour
{
    [Header("Ground Settings")]
    public LayerMask groundLayer;
    public float footOffset = 0.1f;   // nâng bàn chân khỏi mặt đất
    public float raycastRange = 1.5f;
    public float moveSpeed = 5f;      // tốc độ xoay mượt

    private Animator anim;
    private Vector3 leftFootPos, rightFootPos;
    private Quaternion leftFootRot, rightFootRot;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Lấy trạng thái ban đầu
        leftFootPos = anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
        rightFootPos = anim.GetBoneTransform(HumanBodyBones.RightFoot).position;
        leftFootRot = anim.GetBoneTransform(HumanBodyBones.LeftFoot).rotation;
        rightFootRot = anim.GetBoneTransform(HumanBodyBones.RightFoot).rotation;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (anim == null || layerIndex != 0) return;

        // 🚫 Nếu đang di chuyển nhanh hoặc đang attack thì bỏ IK
        if (anim.GetFloat("InputMagnitude") >= 0.3f || GetComponent<PlayerAttackController>().isAttacking)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
            return;
        }

        // ✅ Chỉ xử lý khi đứng yên / chậm
        UpdateFootIK(AvatarIKGoal.LeftFoot, HumanBodyBones.LeftFoot, ref leftFootPos, ref leftFootRot);
        UpdateFootIK(AvatarIKGoal.RightFoot, HumanBodyBones.RightFoot, ref rightFootPos, ref rightFootRot);
    }

    void UpdateFootIK(AvatarIKGoal foot, HumanBodyBones bone, ref Vector3 currentPos, ref Quaternion currentRot)
    {
        Transform footBone = anim.GetBoneTransform(bone);
        Vector3 origin = footBone.position + Vector3.up * 0.3f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastRange, groundLayer))
        {
            // Vị trí mục tiêu: luôn bám đất
            Vector3 targetPos = hit.point + Vector3.up * footOffset;
            currentPos = targetPos;

            // 👉 Dùng hướng nhìn của nhân vật thay vì hướng bàn chân
            Vector3 forward = transform.forward;

            // Xoay bàn chân bám theo mặt đất
            Quaternion targetRot = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(forward, hit.normal), // forward nằm trên mặt phẳng đất
                hit.normal                                   // up theo normal mặt đất
            );

            // Xoay mượt
            currentRot = Quaternion.Slerp(currentRot, targetRot, Time.deltaTime * moveSpeed);

            // Apply IK
            anim.SetIKPositionWeight(foot, 1f);
            anim.SetIKRotationWeight(foot, 1f);
            anim.SetIKPosition(foot, currentPos);
            anim.SetIKRotation(foot, currentRot);
        }
        else
        {
            // Không có mặt đất -> tắt IK
            anim.SetIKPositionWeight(foot, 0f);
            anim.SetIKRotationWeight(foot, 0f);
        }
    }
}
