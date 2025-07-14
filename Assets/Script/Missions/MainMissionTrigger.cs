using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMissionTrigger : MonoBehaviour
{
    public Mission mainMission;
    private bool isTriggered = false;
    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            if (MissionManager.instance == null)
                Debug.LogError("MissionManager.Instance is null!");

            if (mainMission == null)
                Debug.LogError("mainMission is not assigned!");

            MissionManager.instance.TriggerMainStoryMission(mainMission);
            isTriggered = true;
        }
    }
}
