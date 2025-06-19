using Invector.vCharacterController;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clipStep;
    public AudioSource attackSource;
    public AudioClip[] attackClipNormal;
    public AudioClip[] attackClipFire;
    Animator animator;

    internal bool isBuff;

    int pos;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AttackPlaySource(int index)
    {
        if (!isBuff)
        {
            if (attackClipNormal == null) return;
            attackSource.PlayOneShot(attackClipNormal[index]);
        }
        else
        {
            if (attackClipFire == null) return;
            attackSource.PlayOneShot(attackClipFire[index]);
        }
    }

    public void playSourceWalk()
    {
        if (GetComponent<PlayerTakeDamge>().noTakeDamge) return;
        if (clipStep.Length == 0) return; // Kiểm tra danh sách có clip không

        source.PlayOneShot(clipStep[pos]); // Phát clip hiện tại

        pos++; // Tăng chỉ mục lên
        if (pos >= clipStep.Length) pos = 0;
    }

}
