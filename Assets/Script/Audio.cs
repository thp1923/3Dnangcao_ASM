using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header("-----------Audio Source------------")]
    public AudioSource source;

    [Header("-----------Audio Clip------------")]
    public AudioClip[] clip;

    [Header("-----------Pitch------------")]
    float pitchFirst;
    
    [Header("-----------Volume------------")]
    public float volume;

    int currentClip;

    // Start is called before the first frame update
    void Start()
    {
        pitchFirst = 1;
        source.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClip(int index)
    {
        source.loop = false;
        source.PlayOneShot(clip[index]);
    }

    public void PlayClipAlways(int index)
    {
        source.loop = true;
        source.clip = clip[index];
        source.Play();
    }

    public void StopPlay(int index)
    {
        source.clip = clip[index];
        source.Stop();
    }
    public void PlayClipAlwaysUpPich(int index, float pitch)
    {
        source.clip = clip[index];
        source.loop = true;
        source.pitch = pitch;
        source.Play();
    }

    public void ResetPitch()
    {
        source.pitch = pitchFirst;
        source.loop = false;
    }
}
