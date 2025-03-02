using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameSession : MonoBehaviour
{
    public bool Active;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameSession>().HeadUpActive(Active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
