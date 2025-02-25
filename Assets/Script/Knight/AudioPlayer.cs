using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public Audio audioP;
    public AudioPlayerRun audioPR;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AudioWalk()
    {
        if(animator.GetFloat("InputMagnitude") < 0.3f) return;
        audioPR.playSourceWalk();
    }
    public void PlayAudio(int index)
    {
        audioP.PlayClip(index);
    }
    public void PlayAudioAlways(int index)
    {
        audioP.PlayClipAlways(index);
    }
    public void PlayAlwaysUpPitch(int index, float pitch)
    {
        audioP.PlayClipAlwaysUpPich(index, pitch);
    }
    public void PlayAudioStop(int index)
    {
        audioP.StopPlay(index);
    }
    public void ResetPitch()
    {
        audioP.ResetPitch();
    }
}
