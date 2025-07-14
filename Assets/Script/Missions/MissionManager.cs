using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public List<Mission> activeMissions = new List<Mission>();
    public MissionNotificationUI notificationUI;

    private void Awake()
    {
        Debug.Log("MissionManager Awake called");
        instance = this;
    }
    public void AddMission(Mission mission)
    {
        activeMissions.Add(mission);
        notificationUI.ShowMission($"New Mission: {mission.missionName}");
    }
    public void TriggerMainStoryMission(Mission mission)
    {
        if (mission.isMainStory)
        {
            AddMission(mission);
        }
    }
}
