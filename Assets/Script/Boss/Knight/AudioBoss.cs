using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoss : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clipStep;
    public AudioSource attackSource;
    public AudioClip[] attackClipFire;
    Animator animator;


    int pos;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AttackPlaySource(int index)
    {
        if (attackClipFire == null) return;
        attackSource.PlayOneShot(attackClipFire[index]);
    }

    public void playSourceWalk()
    {
        if (clipStep.Length == 0) return; // Ki?m tra danh sách có clip không

        source.PlayOneShot(clipStep[pos]); // Phát clip hi?n t?i

        pos++; // T?ng ch? m?c lên
        if (pos >= clipStep.Length) pos = 0;
    }
}
