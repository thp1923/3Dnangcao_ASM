using Invector.vCharacterController;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clipStep;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    int pos;
    public void playSourceWalk()
    {
        if (animator.GetFloat("InputMagnitude") < 0.5f || GetComponent<PlayerTakeDamge>().noTakeDamge) return;
        if (clipStep.Length == 0) return; // Kiểm tra danh sách có clip không

        source.PlayOneShot(clipStep[pos]); // Phát clip hiện tại

        pos++; // Tăng chỉ mục lên
        if (pos >= clipStep.Length) pos = 0;
    }

}
