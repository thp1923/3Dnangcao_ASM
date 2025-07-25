using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public Mission mission;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mission.state == MissionState.NotStarted) 
        {
            Debug.Log("Collider Player triggered");
            mission.state = MissionState.InProgress;
        }
    }
}
