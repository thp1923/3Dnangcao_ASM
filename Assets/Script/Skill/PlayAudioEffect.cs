using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioEffect : MonoBehaviour
{
    public Audio audioEf;
    // Start is called before the first frame update
    void Start()
    {
        audioEf.PlayClipAlways(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
