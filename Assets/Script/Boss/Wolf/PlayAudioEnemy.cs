using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioEnemy : MonoBehaviour
{
    public Audio audioE;
    // Start is called before the first frame update
    void Start()
    {
        if(audioE == null)
        {
            audioE = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(int index, int indexSource)
    {
        audioE.PlayClip(index, indexSource);
    }
    public void PlayAudioAlways(int index, int indexSource)
    {
        audioE.PlayClipAlways(index, indexSource);
    }
    public void PlayAlwaysUpPitch(int index, int indexSource, float pitch)
    {
        audioE.PlayClipAlwaysUpPich(index, indexSource, pitch);
    }
    public void PlayAudioStop(int index, int indexSource)
    {
        audioE.StopPlay(index, indexSource);
    }
    public void ResetPitch(int indexSource)
    {
        audioE.ResetPitch(indexSource);
    }

}
