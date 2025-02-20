using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header("-----------Audio Source------------")]
    public AudioSource[] source;

    [Header("-----------Audio Clip------------")]
    public AudioClip[] clip;

    [Header("-----------Pitch------------")]
    float pitchFirst;
    
    [Header("-----------Volume------------")]
    public float volume;

    // Start is called before the first frame update
    void Start()
    {
        pitchFirst = 1;
        foreach (AudioSource srce in source)
        {
            srce.volume = volume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClip(int index, int indexSource)
    {
        source[indexSource].loop = false;
        source[indexSource].clip = clip[index];
        source[indexSource].Play();
        if (!source[indexSource].isPlaying && source[indexSource].clip != null)
        {
            source[indexSource].Play();
            Debug.Log(clip[index], source[indexSource]);
        }
    }

    public void PlayClipAlways(int index, int indexSource)
    {
        source[indexSource].loop = true;
        source[indexSource].clip = clip[index];
        source[indexSource].Play();
    }

    public void StopPlay(int index, int indexSource)
    {
        source[indexSource].clip = clip[index];
        source[indexSource].Stop();
    }
    public void PlayClipAlwaysUpPich(int index, int indexSource, float pitch)
    {
        source[indexSource].clip = clip[index];
        source[indexSource].loop = true;
        source[indexSource].pitch = pitch;
        source[indexSource].Play();
    }

    public void ResetPitch(int indexSource)
    {
        source[indexSource].pitch = pitchFirst;
        source[indexSource].loop = false;
    }
}
