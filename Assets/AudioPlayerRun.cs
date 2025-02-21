using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerRun : MonoBehaviour
{
    public AudioSource source;

    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        source.clip = clip;
        source.Play();
        if (!source.isPlaying || source.clip != null)
        {
            source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
