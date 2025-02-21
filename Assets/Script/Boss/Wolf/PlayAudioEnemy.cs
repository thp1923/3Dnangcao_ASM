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

    public void StopAudio(int index)
    {
        audioE.PlayClip(index);
    }
    public void PlayAudio(int index)
    {
        audioE.PlayClip(index);
    }
    public void PlayAlwaysUpPitch(int index, float pitch)
    {
        audioE.PlayClipAlwaysUpPich(index, pitch);
    }
    public void PlayAudioStop(int index)
    {
        audioE.StopPlay(index);
    }
    public void ResetPitch()
    {
        audioE.ResetPitch();
    }

}
