using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject UntilOj;
    public Transform UntilSpawm;
    public void Until()
    {
        GetComponent<AudioPlayer>().PlayAudio(6);
        Instantiate(UntilOj, UntilSpawm.position, UntilSpawm.rotation);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
