using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarukAudio : MonoBehaviour
{
    public AudioSource sourceStep;
    public AudioClip[] clipStep;

    public AudioSource sourceGrowl;
    public AudioClip[] clipGrowl;

    public AudioSource sourceAttack;
    public AudioClip[] clipAttack;

    int pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GrowlPlaySource(int index)
    {
        if (clipGrowl == null) return;
        sourceGrowl.loop = false;
        sourceGrowl.PlayOneShot(clipGrowl[index]);
    }

    public void GrowlLoop(int index)
    {
        if (clipGrowl == null) return;
        sourceGrowl.clip = clipGrowl[index];
        sourceGrowl.loop = true;
        sourceGrowl.Play();
    }

    public void AttackPlaySource(int index)
    {
        if (clipAttack == null) return;
        sourceAttack.PlayOneShot(clipAttack[index]);
    }

    public void playSourceWalk()
    {
        if (clipStep.Length == 0) return;

        sourceStep.PlayOneShot(clipStep[pos]);

        pos++;
        if (pos >= clipStep.Length) pos = 0;
    }
}
