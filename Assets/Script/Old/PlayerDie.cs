using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    public AttributesManager atm;
    public GameObject GameOver;
    int hp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hp = atm.Health;
        if(hp <= 0)
        {
            GameOver.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
