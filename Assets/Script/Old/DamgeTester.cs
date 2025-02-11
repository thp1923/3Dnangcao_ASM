using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamgeTester : MonoBehaviour
{
    public AttributesManager player_atm;
    public AttributesManager enemy_atm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            player_atm.DealDamge(enemy_atm.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            enemy_atm.DealDamge(player_atm.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            var x = player_atm.transform.position;
            DamgePopUPGenerator.current.createPopUp(transform.position
            + new Vector3(UnityEngine.Random.Range(-0.7f, 0.7f), 5f, 0), Random.Range(0, 100).ToString());
        }
    }
}
