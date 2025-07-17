using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoss : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clipStep;
    public AudioSource attackSource;
    public AudioClip[] attackClipFire;
    public AudioClip deathClip;
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

    public void DeathPlaySource()
    {
        attackSource.PlayOneShot(deathClip);
    }

    public void playSourceWalk()
    {
        if (clipStep.Length == 0) return;

        source.PlayOneShot(clipStep[pos]);

        pos++;
        if (pos >= clipStep.Length) pos = 0;
    }
}
